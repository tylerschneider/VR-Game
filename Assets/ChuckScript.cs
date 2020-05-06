using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuckScript : MonoBehaviour
{
    public string Script = @"JCRev r => dac;
    Clarinet cl => r;
    .75 => r.gain;
    0.05 => r.mix;
    0.5 => float velocity;

    fun void playRise(int startNoteMin, int startNoteMax, int increase, int iterations, int speed, float vFreq, float vGain)
    {
        Math.random2(startNoteMin, startNoteMax) => int startNote;
        for (int i; i < iterations; i++)
        {
            playNote(12 + (startNote + increase * i), vFreq, vGain);
            speed::ms => now;
        }
    }

    fun void playSeq(int notes[], int speed, float vFreq, float vGain)
    {
        for (int i; i < notes.size(); i++)
        {
            playNote(12 + notes[i], vFreq, vGain);
            speed::ms => now;
        }
    }

    fun void playNote(float note, float vFreq, float vGain)
    {
        vFreq => cl.vibratoFreq;
        vGain => cl.vibratoGain;
        Std.mtof(note) => cl.freq;
        velocity => cl.noteOn;
    }";

    public string powerup =
    @"fun void powerup(int startNote, int rise, int speed)
    {
        playRise(startNote, startNote + 10, rise, 10, speed, 0, 0.05);
    }";

    public string mystery =
    @"fun void mystery(int startNote, int rise, int speed)
    {
        playRise(startNote, startNote, 4, 5, speed, 10, 0.7);
        playRise(startNote + rise, startNote + rise, 4, 5, speed, 11, 0.8);
        playRise(startNote + rise * 2, startNote + rise * 2, 4, 5, speed, 12, 0.9);
    }";

    public string boss =
    @"fun void boss(int sn, int speed, int speedIncrease)
    {
        playSeq([sn, sn + 2, sn + 3, sn, sn + 3, sn + 5, sn + 6], speed, 5, 0);
        playSeq([sn, sn + 2, sn + 3, sn, sn + 3, sn + 5, sn + 6], speed - speedIncrease, 5, 0);
        playSeq([sn, sn + 2, sn + 3, sn, sn + 3, sn + 5, sn + 6], speed - speedIncrease * 2, 5, 0);
        playSeq([sn, sn + 2, sn + 3, sn, sn + 3, sn + 5, sn + 6], speed - speedIncrease * 3, 5, 0);
        playSeq([sn, sn + 2, sn + 3, sn, sn + 3, sn + 5, sn + 6], speed - speedIncrease * 3, 5, 0);
        playSeq([sn + 6], 750, 5, 0);
    }";

    public string dead =
    @"fun void dead()
    {
        playSeq([55, 52, 54, 51, 53, 50, 52, 49, 51, 48, 50, 47, 49, 46, 48, 45, 47, 44, 46, 43, 36, 37, 36, 37, 36, 37, 36, 37, 36, 37, 36, 36], 80, 10, 1);
    }";

    public string coin = 
    @"fun void coin(int sn, int speed)
    {
        playSeq([sn, sn + 4], speed, 0, 0);
    }";

    /*//powerup
    powerup(40, 3, 50);
    pause(0.5);

    //powerdown
    powerup(60, -2, 100);
    pause(0.5);

    //mystery
    mystery(40, 10, 350);
    pause(0.5);

    //boss enter
    boss(40, 150, 25);
    pause(0.5);

    //dead
    dead();
    pause(1);

    //coin
    coin(75, 100);
    pause(1);*/

}
