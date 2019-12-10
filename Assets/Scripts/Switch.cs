using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
  [Tooltip("Attach MovingPlatform Object here. This switch will turn on that platform's movement when activated.")]
  public GameObject TargetPlatform;

  [Tooltip("Child gameObject for detecting if button was pressed.")]
  public GameObject SwitchPress; // what gameobject is the stunnedCheck

  [HideInInspector]
  public bool isPressed = false;

  public string pressedLayer = "Pressed";  // name of the layer to put enemy on when stunned
  public string playerLayer = "Player";  // name of the player layer to ignore collisions with when stunned

  // SFXs
  public AudioClip pressedSFX;

  // stores references to components
  Animator _animator;
  AudioSource _audio;

  // store the layer number the switch is on (setup in Awake)
  int _switchLayer;

  // layer switch goes when pressed, to avoid further collision
  int _pressedLayer;

  // Start is called before the first frame update
  void Awake(){
    // grabs animator
    _animator = GetComponent<Animator>();
    if (_animator==null){ // if Animator is missing
      Debug.LogError("Animator component missing from this gameobject");
    }
    // grabs audio source
    _audio = GetComponent<AudioSource> ();
    if (_audio==null) { // if AudioSource is missing
      Debug.LogWarning("AudioSource component missing from this gameobject. Adding one.");
      // let's just add the AudioSource component dynamically
      _audio = gameObject.AddComponent<AudioSource>();
    }

    // checks if moving platform is attatched. Switch script is useless without
    if(TargetPlatform==null){
      Debug.LogError("Moving Platform must be attatched to setup the Switch");
    }

    // makes sure platform is not activated
    if(TargetPlatform.gameObject.GetComponent<PlatformMover>().enabled){
      TargetPlatform.gameObject.GetComponent<PlatformMover>().enabled = false;
    }

    // checks for child object SwitchPress
    if (SwitchPress==null) {
      Debug.LogError("stunnedCheck child gameobject needs to be setup on the enemy");
    }

    // determine the switch's specified layer
    _switchLayer = this.gameObject.layer;

		// determine the stunned enemy layer number
		_pressedLayer = LayerMask.NameToLayer(pressedLayer);

		// make sure collision are off between the playerLayer and the stunnedLayer
		// which is where the enemy is placed while stunned
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(playerLayer), _pressedLayer, true);
  }

  // play sound through the audiosource on the gameobject
  void playSound(AudioClip clip){
    _audio.PlayOneShot(clip);
  }

  // when SwitchPress child is pressed, run this.
  // Will switch sprite to pressed switch Sprite
  // as well as activate platform that this switch is linked to
  public void TurnedOn(){
    if(!isPressed){
      playSound(pressedSFX);
      _animator.SetTrigger("Pressed");

      // the code that actually activates the platform's movement
      TargetPlatform.gameObject.GetComponent<PlatformMover>().enabled = true;

      // makes switch unable to collide with player once pressed
      this.gameObject.layer = _pressedLayer;
      SwitchPress.layer = _pressedLayer;
    }
  }
}
