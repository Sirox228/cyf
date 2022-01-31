﻿using UnityEngine;

public class BlueSoul : AbstractSoul {
    private bool jumping;
    private bool falling = true;
    private float ySpeed;
    private const int jumpYSpeed = 60 * 4;
    private const int maxFallSpeed = 60 * 9;
    private const int jumpDecelerationSpeed = 60 * 7;
    private int fallSpeed; // appears to increase per update and then reset
    private const int fallSpeedIncrement = 60;

    public BlueSoul() {
        fallSpeed = -fallSpeedIncrement; // mildly filthy hack to assure one frame of midair hold
    }

    public override Color color {
        get { return Color.blue; }
    }

    public override Vector2 GetMovement(float xDir, float yDir) {
        // pressing up arrow
        if (yDir == 1 &&!jumping &&!falling) {
            ySpeed = jumpYSpeed;
            jumping = true;
            falling = false;
        }

        // releasing up arrow
        if (yDir == 0 && jumping &&!falling) {
            if (ySpeed > 0)
                ySpeed = 0;
            jumping = false;
            falling = true;
        }

        // while midair
        if (jumping)
            ySpeed -= jumpDecelerationSpeed * Time.deltaTime;

        if (falling) {
            fallSpeed += fallSpeedIncrement;
            ySpeed -= fallSpeed * Time.deltaTime;
        }

        // turning point
        if (ySpeed <= 0) {
            jumping = false;
            falling = true;
        }

        // if fall speed is too high
        if (ySpeed < -maxFallSpeed)
            ySpeed = -maxFallSpeed;

        return new Vector2(xDir * speed, ySpeed);
    }

    public override void PostMovement(float xDelta, float yDelta) {
        if (!falling || fallSpeed <= 0 || yDelta != 0.0f) return;
        falling   = false;
        fallSpeed = -fallSpeedIncrement;
    }
}