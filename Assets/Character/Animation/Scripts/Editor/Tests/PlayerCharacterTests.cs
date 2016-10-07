using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using Poptropica2.Characters;
using Spine.Unity;


[TestFixture]
public class PlayerCharacterTests {
	GameObject character;

	[SetUp]		//Using TestFixtureSetUp seems to allow the state of references in these tests to persist even when the state of the scene changes - so SetUp for each test.
	public void PlayerCharacterTestsSetup() {
		character = GameObject.FindWithTag("Player");
		Debug.Log("PlayerCharacterTests running on " + character.name + ", id: " + character.GetInstanceID());
	}

	[TearDown]
	public void PlayerCharacterTestsTearDown() {
		character = null;
	}

	[Test]
	public void CharacterIsInSceneTest() {
		Assert.NotNull(character, "No object tagged Player was found in scene.");
	}

	public void CharacterModelExistsTest() {
		CharacterModel model = character.GetComponent<CharacterModel>();
		Assert.NotNull(model, "A component derived from Poptropica2.Characters.CharacterModel must be attached to the player");
	}

	[Test]
	public void BodyColliderExistsTest() {
		List<Collider2D> colliders = new List<Collider2D>(character.GetComponentsInChildren<Collider2D>());
		Collider2D bodyCollider = colliders.Find(x => x.gameObject.layer == LayerMask.NameToLayer("CharacterBody"));
		Assert.NotNull(bodyCollider, "A collider belonging to the layer CharacterBody must be attached to a child of the character.");
    }
	 
	[Test]
	public void BodyColliderConnectedTest() {
		CharacterModel model = character.GetComponent<CharacterModel>();
		Assert.NotNull(model.bodyCollider, "The body collider must be referenced by the CharacterModel.");
	}

	GameObject primaryCollider = GameObject.FindWithTag("PrimaryPlayerCollider");
	[Test]
	public void PrimaryColliderExistsTest() {
		Assert.NotNull(primaryCollider, "No GameObject tagged as primary collider was found in the scene.");
	}

	[Test]
	public void PrimaryColliderConnectedTest() {
		CharacterModel model = character.GetComponent<CharacterModel>();
		Assert.NotNull(model.primaryCollider, "No reference to the Primary Collider on the CharacterModel.");
		Assert.AreSame(primaryCollider.GetComponent<Collider2D>(), model.primaryCollider, "The model's primary collider does not refer to the tagged primary player collider in the scene");
	}

	[Test]
	public void PrimaryColliderLayerTest() {
		Assert.AreEqual(LayerMask.NameToLayer("Characters"), primaryCollider.layer,
			"The primary collider layer was \"" + LayerMask.LayerToName(primaryCollider.layer) + "\", but should have been \"Characters\".");
	}


	[Test]
	public void AnimatorExistsTest() {
		Animator[] animators = character.GetComponentsInChildren<Animator>();
		Assert.AreEqual(1, animators.Length, "There must be one (and only one) animator attached to the character to use as a state machine");
	}

	[Test]
	public void AnimatorUpdatePhysicsModeTest() {
		Animator animator = character.GetComponentInChildren<Animator>();
		Assert.AreEqual(AnimatorUpdateMode.AnimatePhysics, animator.updateMode, "Character's Animator (state machine) was not in AnimatePhysics mode.");
	}

	[Test]
	public void SkeletonAnimatorExistsTest() {
		SkeletonAnimator skeletonAnimator = character.GetComponentInChildren<SkeletonAnimator>();
		Assert.NotNull(skeletonAnimator, "There must be a SkeletonAnimator component in the character hierarchy.");
	}

	[Test]
	public void SkeletonAnimatorEyesAttachedTest() {
		SkeletonAnimator skeletonAnimator = character.GetComponentInChildren<SkeletonAnimator>();
		List<BoneFollower> boneFollowers = new List<BoneFollower>(skeletonAnimator.GetComponentsInChildren<BoneFollower>());
		GameObject eyesAttach = boneFollowers.Find(x => x.name == "EyesAttach").gameObject;
		Assert.NotNull(eyesAttach, "There must be a BoneFollower component on an object called EyesAttach, parented to the SkeletonAnimator of the character.");
	}

	[Test]
	public void PupilOffsetterExistsTest() {
		SkeletonAnimator skeletonAnimator = character.GetComponentInChildren<SkeletonAnimator>();
		List<BoneFollower> boneFollowers = new List<BoneFollower>(skeletonAnimator.GetComponentsInChildren<BoneFollower>());
		GameObject eyesAttach = boneFollowers.Find(x => x.name == "EyesAttach").gameObject;
		PupilOffsetter pupilOffsetter = eyesAttach.GetComponentInChildren<PupilOffsetter>();
		Assert.NotNull(pupilOffsetter, "There must be a PupilOffsetter object parented to EyeAttach.");
	}

	[Test]
	public void PupilsAttachedTest() {
		SkeletonAnimator skeletonAnimator = character.GetComponentInChildren<SkeletonAnimator>();
		List<BoneFollower> boneFollowers = new List<BoneFollower>(skeletonAnimator.GetComponentsInChildren<BoneFollower>());
		GameObject eyesAttach = boneFollowers.Find(x => x.name == "EyesAttach").gameObject;
		PupilOffsetter pupilOffsetter = eyesAttach.GetComponentInChildren<PupilOffsetter>();
		Assert.NotNull(pupilOffsetter.pupil, "The pupil sprite must be referenced by the PupilOffsetter component");
	}

	[Test]
	public void RigidbodyExistsTest() {
		Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
		Assert.NotNull(rb, "There must be a Rigidbody2D attached to the character.");
	}

	[Test]
	public void RigidbodyFreezeZRotationTest() {
		Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
		Assert.True(rb.freezeRotation, "Character's rigidbody rotation must be frozen to prevent the physics system from tipping it over.");
	}

	[Test]
	public void CharacterControllerReferencedTest() {
		CharacterModel model = character.GetComponent<CharacterModel>();
		Assert.NotNull(model.controllerContainer.Result, "No controller is referenced by the character. \n" +
			"Make sure that a controller object is present in the scene to capture user input,\nand that the Controller Container component of the CharacterModel refers to it.");
	}

	[Test]
	public void GroundRaycastersReferencedTest() {
		CharacterModel model = character.GetComponent<CharacterModel>();
		Assert.Greater(model.groundRaycasters.Length, 1, "There must be at least two ground raycaster transforms referenced by the CharacterModel.");
		Assert.That(model.groundRaycasters, Is.All.Not.Null, "One or more ground raycaster references on the CharacterModel was null");
	}

	[Test]
	public void LateralRaycastersReferencedTest() {
		CharacterModel model = character.GetComponent<CharacterModel>();
		Assert.Greater(model.lateralRaycasters.Length, 0, "There must be at least one lateral raycaster transform referenced by the CharacterModel.");
		Assert.That(model.lateralRaycasters, Is.All.Not.Null, "One or more ground raycaster references on the CharacterModel was null");
	}
	/**
	//These tests should be moved to the customization system
	[Test]
	public void EyelidSpriteAnimatorSingleGenderTest() {
		EyelidSpriteAnimator[] eyelidSpriteAnimators = character.GetComponentsInChildren<EyelidSpriteAnimator>();
		Assert.AreEqual(1, eyelidSpriteAnimators.Length, "Only one gender's eyelids should be active at one time.");
	}

	[Test]
	public void EyelidSpriteAnimatorPupilFollowSpritesReferencedTest() {
		EyelidSpriteAnimator eyelidSpriteAnimator = character.GetComponentInChildren<EyelidSpriteAnimator>();
		Assert.That(eyelidSpriteAnimator.pupilFollowSprites, Is.All.Not.Null, "There are missing references in the Pupil Follow Sprites of the active Eyelid Sprite Animator");
	}

	[Test]
	public void EyelidSpriteAnimatorBlinkSpritesReferencedTest() {
		EyelidSpriteAnimator eyelidSpriteAnimator = character.GetComponentInChildren<EyelidSpriteAnimator>();
		Assert.That(eyelidSpriteAnimator.blinkSequence, Is.All.Not.Null, "There are missing references in the Blink Sequence Sprites of the active Eyelid Sprite Animator");
	}

	[Test]
	public void EyelidSpriteAnimatorRearLashesExistsTest() {
		EyelidSpriteAnimator eyelidSpriteAnimator = character.GetComponentInChildren<EyelidSpriteAnimator>();
		GameObject staticRearLash = eyelidSpriteAnimator.transform.FindChild("RearLashesSprite").gameObject;
		Assert.NotNull(staticRearLash, 
			"There must be a GameObject named RearLashesSprite underneath the active EyelidSpriteAnimator. It may be empty or have its sprite renderer disabled, but it must exist.");
	}
	/**/
}
