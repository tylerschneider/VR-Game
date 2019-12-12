using System;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public GameObject objectSlot;
    private bool slotted = true;

    [Tooltip("Freeze position on grab begin to make the object stay in hand, mainly used for sword")]
    public bool freezePositionOnGrabBegin = false;
    public bool freezePositionOnGrabEnd = false;
    [Tooltip("Objects should freeze rotation on grab begin unless hinged, which should freeze on end")]
    public bool freezeRotationOnGrabBegin = true;
    public bool freezeRotationOnGrabEnd = false;
    public bool makeKinematic = false;

    public bool allowOffhandGrab = true;
    public bool allowDistanceGrab = true;
    public string snapPosition;
    public bool parentToHand = false;
    public bool addSpringJoint = true;
    public Collider[] m_grabPoints = null;

    public float minVibrateDistance = 0.02f;
    public float maxSpringDistance = 0.2f;

    private bool addedRigidbody = false;

    protected bool m_grabbedKinematic = false;
    protected bool m_grabbedGravity = true;
    protected Transform m_grabbedParent = null;
    protected Collider m_grabbedCollider = null;
    public Grabber m_grabbedBy = null;

    public bool isGrabbed
    {
        get { return m_grabbedBy != null; }
    }

    /// <summary>
    /// Returns the OVRGrabber currently grabbing this object.
    /// </summary>
    public Grabber grabbedBy
    {
        get { return m_grabbedBy; }
    }

    /// <summary>
    /// The transform at which this object was grabbed.
    /// </summary>
    public Transform grabbedTransform
    {
        get { return m_grabbedCollider.transform; }
    }

    /// <summary>
    /// The Rigidbody of the collider that was used to grab this object.
    /// </summary>
    public Rigidbody grabbedRigidbody
    {
        get { return m_grabbedCollider.attachedRigidbody; }
    }

    /// <summary>
    /// The contact point(s) where the object was grabbed.
    /// </summary>
    public Collider[] grabPoints
    {
        get { return m_grabPoints; }
    }

    virtual public void GrabBegin(Grabber hand, Collider grabPoint)
    {
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;

        if(TryGetComponent<Rigidbody>(out Rigidbody rb) == false)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            addedRigidbody = true;
        }
        m_grabbedGravity = rb.useGravity;
        rb.useGravity = false;

        if(!transform.parent || transform.parent.name != "OVRPlayerController")
        {
            m_grabbedKinematic = rb.isKinematic;
        }

        if (transform.parent != null && transform.parent.name != "OVRPlayerController")
        {
            m_grabbedParent = transform.parent;
        }

        if(snapPosition != null && snapPosition != "")
        {
            transform.position = m_grabbedBy.transform.Find(snapPosition).position;
            transform.rotation = m_grabbedBy.transform.Find(snapPosition).rotation;
        }

        if(parentToHand)
        {
            transform.parent = hand.transform;
        }

        if(addSpringJoint)
        {
            ConfigurableJoint cj = gameObject.AddComponent<ConfigurableJoint>();
            cj.connectedBody = m_grabbedBy.GetComponent<Rigidbody>();
            cj.autoConfigureConnectedAnchor = false;
            cj.anchor = transform.InverseTransformPoint(m_grabbedBy.transform.position);
            cj.connectedAnchor = Vector3.zero;
            cj.xMotion = cj.yMotion = cj.zMotion = ConfigurableJointMotion.Limited;

            if (freezePositionOnGrabBegin)
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
            if (freezePositionOnGrabEnd)
            {
                gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
            }
            if (freezeRotationOnGrabBegin)
            {
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            }
            if (freezeRotationOnGrabEnd)
            {
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            }

        }

        if(makeKinematic == true)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    virtual public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        OVRInput.SetControllerVibration(0, 0, m_grabbedBy.m_controller);

        if (parentToHand)
        {
            transform.parent = m_grabbedParent;
        }

        if (addSpringJoint)
        {
            Destroy(GetComponent<ConfigurableJoint>());

            if (freezePositionOnGrabBegin)
            {
                gameObject.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePosition;
            }
            if (freezePositionOnGrabEnd)
            {
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
            if (freezeRotationOnGrabBegin)
            {
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            }
            if (freezeRotationOnGrabEnd)
            {
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            }

        }

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = m_grabbedKinematic;
        rb.useGravity = m_grabbedGravity;
        rb.velocity = linearVelocity;
        rb.angularVelocity = angularVelocity;

        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    private void FixedUpdate()
    {
        if(isGrabbed)
        {
            float dist = Vector3.Distance(transform.InverseTransformPoint(m_grabbedBy.transform.position), GetComponent<ConfigurableJoint>().anchor);
            
            if (dist > minVibrateDistance)
            {
                OVRInput.SetControllerVibration(1, dist  / maxSpringDistance, m_grabbedBy.m_controller);
            }
            else
            {
                OVRInput.SetControllerVibration(0, 0, m_grabbedBy.m_controller);
            }


            if (dist >= maxSpringDistance)
            {
                m_grabbedBy.ForceRelease(this);
            }

            slotted = false;
        }

        if (objectSlot != null && GetComponent<GrabbableObject>().isGrabbed == false)
        {
            if (objectSlot.GetComponent<MeshRenderer>().enabled == true)
            {
                if (addedRigidbody == true)
                {
                    Destroy(GetComponent<Rigidbody>());
                }

                objectSlot.GetComponent<MeshRenderer>().enabled = false;

                slotted = true;
            }
            else if (slotted == false)
            {
                transform.parent = null;
            }
        }

        if(slotted == true && objectSlot != null)
        {
            transform.position = objectSlot.transform.position;
            transform.rotation = objectSlot.transform.rotation;
            transform.parent = objectSlot.transform.parent;
        }
    }

    void Awake()
    {
        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
                throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }
    }

    void OnDestroy()
    {
        if (m_grabbedBy != null)
        {
            // Notify the hand to release destroyed grabbables
            m_grabbedBy.ForceRelease(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(objectSlot != null)
        {
            if(other.gameObject == objectSlot && GetComponent<GrabbableObject>().isGrabbed == true)
            {
                objectSlot.GetComponent<MeshRenderer>().enabled = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (objectSlot != null)
        {
            if (other.gameObject == objectSlot && GetComponent<GrabbableObject>().isGrabbed == true)
            {
                objectSlot.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
