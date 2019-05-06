using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    Animator animator;
    AnimatorControllerParameter[] parameter;
    public AnimatorTransition aT;

    float Blent
    {
        get { return animator.parameters[0].defaultFloat; }
        set { animator.parameters[0].defaultFloat = value; }
    }
    bool ToForward
    {
        get { return animator.parameters[1].defaultBool; }
        set { animator.SetBool("ToForward", value);  }
    }
    bool ToBackwards
    {
        get { return animator.parameters[2].defaultBool; }
        set { animator.SetBool("ToBackwards", value); }
    }
    bool ToRight
    {
        get { return animator.parameters[3].defaultBool; }
        set { animator.SetBool("ToRight", value); }
    }
    bool ToLeft
    {
        get { return animator.parameters[4].defaultBool; }
        set { animator.SetBool("ToLeft", value); }
    }
    bool Dying
    {
        get { return animator.parameters[5].defaultBool; }
        set { animator.SetBool("Dying", value); }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        parameter = animator.parameters;
        for (int i = 1; i < animator.parameters.Length; i++)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsTag("1"));
            var v = animator.GetCurrentAnimatorStateInfo(0);
            if (animator.GetBool(GetParameterName(i)))
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(GetAnimationnameIndex(i)))
                {
                    SetBoolValue(i, false);
                }
            }
        }

        aT = new AnimatorTransition
        {
            mute = true
        };
        
        

        if (Input.GetKey(KeyCode.W))
        {
            ToForward = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            ToLeft = true;
        }
        if (Input.GetKey(KeyCode.X))
        {
            ToBackwards = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ToRight = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Dying = true;
        }
    }

    private string GetParameterName(int _index)
    {
        string toReturn = "";
        switch (_index)
        {
            case 0:
                toReturn = "Blend";
                break;
            case 1:
                toReturn = "ToForward";
                break;
            case 2:
                toReturn = "ToBackwards";
                break;
            case 3:
                toReturn = "ToRight";
                break;
            case 4:
                toReturn = "ToLeft";
                break;
            case 5:
                toReturn = "Dying";
                break;
            default:
                throw new System.Exception("Index " + _index + " is not available");
                break;
        }
        return toReturn;
    }

    private string GetAnimationnameIndex(int _index)
    {
        string toReturn = "";
        switch (_index)
        {
            case 1:
                toReturn = "Rifle Run";
                break;
            case 2:
                toReturn = "Run Backwards";
                break;
            case 3:
                toReturn = "Run Right";
                break;
            case 4:
                toReturn = "Run Left";
                break;
            case 5:
                toReturn = "Dying";
                break;
            default:
                throw new System.Exception("Index " + _index + " is not available");
                break;
        }
        return toReturn;
    }

    private void SetBoolValue(int _index, bool _value)
    {
        switch (_index)
        {
            case 1:
                ToForward = _value;
                break;
            case 2:
                ToBackwards = _value;
                break;
            case 3:
                ToRight = _value;
                break;
            case 4:
                ToLeft = _value;
                break;
            case 5:
                Dying = _value;
                break;
            default:
                break;
        }
    }
}
