using Godot;
using System;

public partial class AnimationWaiter : Node
{
    /// <summary>
    /// We run this method after all animations have finished
    /// </summary>
    Action AnimationReturnMethod;

    /// <summary>
    /// If we are currently waiting on our animations before running a method
    /// </summary>
    bool AnimationsInProgress;

    /// <summary>
    /// Total animations created
    /// </summary>
    public int AnimationsCount = 0;

    /// <summary>
    /// Animations that have completed
    /// </summary>
    int _AnimationsFinished = 0;
    public int AnimationsFinished
    {
        get => _AnimationsFinished;
        set
        {
            _AnimationsFinished = value;
            CheckAnimations();
        }
    }

    /// <summary>
    /// Start a  animation, specifying the method to call after it ends
    /// </summary>
    /// <param name="action"></param>
    public void StartAnimation(Action action)
    {
        if (AnimationsInProgress)
        {
            GD.PushWarning($"Attempted to start  Animation while already animating!" +
                $"\nNew method: {action.Method.Name}" +
                $"\nOld method: {AnimationReturnMethod.Method.Name}");
        }

        if (AnimationsCount == 0)
        {
            GD.PushWarning($" animation started with no animations! Terminating immediately." +
                $"\nSpecified return method: {action.Method.Name}");
        }

        AnimationReturnMethod = action;
        AnimationsInProgress = true;

        CheckAnimations();
    }

    /// <summary>
    /// Add an animation to the counter
    /// </summary>
    public void AddAnimation()
    {
        AnimationsCount++;
    }

    /// <summary>
    /// Mark previously-added animation as finished
    /// </summary>
    public void FinishAnimation()
    {
        AnimationsFinished++;
    }

    /// <summary>
    /// Check if we should end our animating and run our method
    /// </summary>
    void CheckAnimations()
    {
        //if we're animating AND have finished all animations: progress!
        if (AnimationsInProgress && AnimationsFinished == AnimationsCount)
        {
            AnimationsCount = 0;
            _AnimationsFinished = 0;
            //DO NOT SET THE PROPERTY HERE OR WE LOOP FOREVER
            AnimationsInProgress = false;

            AnimationReturnMethod.Invoke();
        }
        else if (AnimationsFinished > AnimationsCount)
        {
            GD.PushError($"Too many  animations finished! Completed {AnimationsFinished} out of {AnimationsCount} animations!");
        }
    }
}
