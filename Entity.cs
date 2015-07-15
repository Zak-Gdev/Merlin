using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Entity : MonoBehaviour {

	public float health;
	public float maxHealth;
	public Image healthBar;
	public float speed;
	public float maxSpeed;
	public Color tint;

	private float damageTime;
	private float damageDuration;

	private bool burning;
	private float burnDamage;
	private float burnOff; //What time does burn wear off

	private bool blinded;
	private float blindOff;

	private bool frozen;
	private float freezeOff;

	public bool invulnerable;

	public Entity(){
		damageDuration = 0.2f;
		damageTime = -1;

		tint = Color.white;

		burning = false;
		blinded = false;
		frozen = false;
		invulnerable = false;
	}

	public bool Burning{ 
		get{
			return burning;
		}
	} 
	public bool Blinded{ 
		get{
			return blinded;
		}
	}
	public bool Frozen {
		get{
			return frozen;
		}
	}

	public void dealDamage(float amount){
		health -= amount;
		updateHealthBar ();
		damageTime = Time.time;
		if (health <= 0) {
			kill ();
		}
	}

	public void heal(float amount){
		health += amount;
		if (health > maxHealth) {
			health = maxHealth;
		}
		updateHealthBar ();
	}


	public void updateDamages(){
		if (damageTime + damageDuration >= Time.time) {
			GetComponentInChildren<SpriteRenderer> ().color = Color.red;
			healthBar.color = Color.red;
		} else {
			GetComponent<SpriteRenderer> ().color = tint;
			healthBar.color = Color.white;
			if(blinded){
				healthBar.color = Color.black;
			}
		}
		if (burning && burnOff <= Time.time ) {
			extinguish();
		}
		if(frozen && freezeOff <= Time.time){
			defrost ();
		}
		if (blinded && blindOff <= Time.time) {
			unblind ();
		}
	}

	public void ignite(float burnDamage, float duration){
		defrost ();
		burning = true;
		tint = Color.yellow;
		this.burnDamage = burnDamage;
		burnOff = Time.time + duration;
		InvokeRepeating ("burn", 1, 1);
	}

	public void burn(){
		dealDamage (burnDamage);
	}

	public void extinguish (){
		CancelInvoke("burn");
		tint = Color.white;
		burning = false;
	}

	public void freeze(float amount, float duration){
		extinguish ();
		frozen = true;
		speed *= amount;
		tint = Color.blue;
		freezeOff = Time.time + duration;
	}

	public void defrost(){
		tint = Color.white;
		frozen = false;
		speed = maxSpeed;
	}

	public void blind(float duration){
		blindOff = Time.time + duration;
		healthBar.color = Color.black;
		blinded = true;
	}

	public void unblind(){
		healthBar.color = Color.white;
		blinded = false;
	}

	public virtual void updateHealthBar(){
		healthBar.fillAmount = health / maxHealth;
	}

	public virtual void kill(){	}

	public virtual void knockback(Vector2 pos){	}


	///test
	public void slow(float amount){
		speed *= amount;
	}


}
