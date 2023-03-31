using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLimits : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 position_clip_space = Camera.main.WorldToViewportPoint(this.gameObject.transform.position); //Save the position of the object based on its position on the screen. (Screen position goes from 0 to 1).
        if (position_clip_space.x > 1) //If the object is further to the right than the screen size
        {
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, position_clip_space.y, position_clip_space.z)); //Put the object to the left of the screen, mantaining its Y and Z values
        }
        else if (position_clip_space.x < 0)//If the object is further to the left than the screen size
        {
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, position_clip_space.y, position_clip_space.z)); //Put the object to the right of the screen, mantaining its Y and Z values
        } 
        if (position_clip_space.y > 1) //If the object is surpassing the bottom limit of the screen size
        {
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position_clip_space.x, 0, position_clip_space.z)); //Put the object to the top of the screen, mantaining its X and Z values
        }
        else if (position_clip_space.y < 0) //If the object is surpassing the top limit of the screen size
        {
            this.gameObject.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(position_clip_space.x, 1, position_clip_space.z)); //Put the object to the bottom of the screen, mantaining its X and Z values
        }  
    }
}
