datablock ProjectileData( AimCityRifleBulletProjectile )
{
   projectileShapeName = "";

   directDamage        = 25;
   radiusDamage        = 0;
   damageRadius        = 0.5;
   areaImpulse         = 0.5;
   impactForce         = 1;

   explosion           = BulletDirtExplosion;
   decal               = BulletHoleDecal;

   muzzleVelocity      = 1000;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 992;
   fadeDelay           = 1472;
   bounceElasticity    = 0;
   bounceFriction      = 0;
   isBallistic         = false;
   gravityMod          = 1;
};

function AimCityRifleBulletProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
{
   // Apply impact force from the projectile.

   // Apply damage to the object all shape base objects
   if ( %col.getType() & $TypeMasks::GameBaseObjectType )
      %col.damage(%obj,%pos,%this.directDamage,"BulletProjectile");
}

function AimCityRifle::onCollision(%this, %obj, %collObj, %vec, %len)
{
  if (%collObj.getClassName() !$= "Player")
  {
    return;
  }

  %db = %collObj.getDataBlock();
  %db.maxInv[AimCityRifle] = 1;
  %db.maxInv[AimCityRifleClip] = 10;

  %collObj.setInventory(AimCityRifle, 1);
  %collObj.setInventory(AimCityRifleClip, %collObj.maxInventory(AimCityRifleClip));
  %collObj.setInventory(AimCityRifleAmmo, %collObj.maxInventory(AimCityRifleAmmo));
  %collObj.addToWeaponCycle(AimCityRifle);
  %collObj.use(AimCityRifle);
}

datablock ItemData(AimCityRifle)
{
  // Mission editor category
  category = "Weapon";

  // Hook into Item Weapon class hierarchy. The weapon namespace
  // provides common weapon handling functions in addition to hooks
  // into the inventory system.
  className = "Weapon";

  // Basic Item properties
  shapeFile = "art/shapes/dotsnetcrits/levels/aimcity/rifle/aimcityrifle.dae";
  mass = 1;
  elasticity = 0.2;
  friction = 0.6;
  emap = true;

  // Dynamic properties defined by the scripts
  //PreviewImage = 'lurker.png';
  pickUpName = "AimCityRifle";
  description = "AimCityRifle";
  image = AimCityRifleImage;
  reticle = "crossHair";
  zoomReticle = "crossHairZoomed";
};

datablock ItemData(AimCityRifleClip)
{
   // Mission editor category
   category = "AmmoClip";

   // Add the Ammo namespace as a parent.  The ammo namespace provides
   // common ammo related functions and hooks into the inventory system.
   className = "AmmoClip";

   // Basic Item properties
   shapeFile = "art/shapes/dotsnetcrits/levels/aimcity/rifle/aimcityrifle.dae";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;

   // Dynamic properties defined by the scripts
   pickUpName = "AimCityRifle clip";
   count = 1;
   maxInventory = 10;
};

datablock ItemData(AimCityRifleAmmo)
{
   // Mission editor category
   category = "Ammo";

   // Add the Ammo namespace as a parent.  The ammo namespace provides
   // common ammo related functions and hooks into the inventory system.
   className = "Ammo";

   // Basic Item properties
   shapeFile = "art/shapes/dotsnetcrits/levels/aimcity/rifle/aimcityrifle.dae";
   mass = 1;
   elasticity = 0.2;
   friction = 0.6;

   // Dynamic properties defined by the scripts
   pickUpName = "AimCityRifle ammo";
   maxInventory = 30;
   clip = AimCityRifleClip;
};

datablock ShapeBaseImageData(AimCityRifleImage)
{
   // Basic Item properties
   shapeFile = "art/shapes/dotsnetcrits/levels/aimcity/rifle/aimcityrifle.dae";
   shapeFileFP = "art/shapes/dotsnetcrits/levels/aimcity/rifle/aimcityrifle.dae";
   emap = true;

   imageAnimPrefix = "Rifle";
   imageAnimPrefixFP = "Rifle";

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   firstPerson = true;
   useEyeNode = true;
   animateOnServer = true;

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   class = "WeaponImage";
   className = "WeaponImage";

   // Projectiles and Ammo.
   item = AimCityRifle;
   ammo = AimCityRifleAmmo;
   clip = AimCityRifleClip;

   projectile = AimCityRifleBulletProjectile;
   projectileType = Projectile;
   projectileSpread = "0.00";

   casing = BulletShell;
   shellExitDir        = "1.0 0.3 1.0";
   shellExitOffset     = "0.15 -0.56 -0.1";
   shellExitVariance   = 15.0;
   shellVelocity       = 3.0;

   // Weapon lights up while firing
   lightType = "WeaponFireLight";
   lightColor = "0.992126 0.968504 0.708661 1";
   lightRadius = "4";
   lightDuration = "100";
   lightBrightness = 2;

   // Shake camera while firing.
   shakeCamera = false;
   camShakeFreq = "0 0 0";
   camShakeAmp = "0 0 0";

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   useRemainderDT = true;

   // Initial start up state
   stateName[0]                     = "Preactivate";
   stateTransitionOnLoaded[0]       = "Activate";
   stateTransitionOnNoAmmo[0]       = "NoAmmo";

   // Activating the gun.  Called when the weapon is first
   // mounted and there is ammo.
   stateName[1]                     = "Activate";
   stateTransitionGeneric0In[1]     = "SprintEnter";
   stateTransitionOnTimeout[1]      = "Ready";
   stateTimeoutValue[1]             = 0.5;
   stateSequence[1]                 = "switch_in";
   stateSound[1]                    = LurkerSwitchinSound;

   // Ready to fire, just waiting for the trigger
   stateName[2]                     = "Ready";
   stateTransitionGeneric0In[2]     = "SprintEnter";
   stateTransitionOnMotion[2]       = "ReadyMotion";
   stateTransitionOnTimeout[2]      = "ReadyFidget";
   stateTimeoutValue[2]             = 10;
   stateWaitForTimeout[2]           = false;
   stateScaleAnimation[2]           = false;
   stateScaleAnimationFP[2]         = false;
   stateTransitionOnNoAmmo[2]       = "NoAmmo";
   stateTransitionOnTriggerDown[2]  = "Fire";
   stateSequence[2]                 = "idle";

   // Same as Ready state but plays a fidget sequence
   stateName[3]                     = "ReadyFidget";
   stateTransitionGeneric0In[3]     = "SprintEnter";
   stateTransitionOnMotion[3]       = "ReadyMotion";
   stateTransitionOnTimeout[3]      = "Ready";
   stateTimeoutValue[3]             = 6;
   stateWaitForTimeout[3]           = false;
   stateTransitionOnNoAmmo[3]       = "NoAmmo";
   stateTransitionOnTriggerDown[3]  = "Fire";
   stateSequence[3]                 = "idle_fidget1";
   stateSound[3]                    = LurkerIdleSound;

   // Ready to fire with player moving
   stateName[4]                     = "ReadyMotion";
   stateTransitionGeneric0In[4]     = "SprintEnter";
   stateTransitionOnNoMotion[4]     = "Ready";
   stateWaitForTimeout[4]           = false;
   stateScaleAnimation[4]           = false;
   stateScaleAnimationFP[4]         = false;
   stateSequenceTransitionIn[4]     = true;
   stateSequenceTransitionOut[4]    = true;
   stateTransitionOnNoAmmo[4]       = "NoAmmo";
   stateTransitionOnTriggerDown[4]  = "Fire";
   stateSequence[4]                 = "run";

   // Fire the weapon. Calls the fire script which does
   // the actual work.
   stateName[5]                     = "Fire";
   stateTransitionGeneric0In[5]     = "SprintEnter";
   stateTransitionOnTimeout[5]      = "NewRound";
   stateTimeoutValue[5]             = 0.15;
   stateFire[5]                     = true;
   stateRecoil[5]                   = "";
   stateAllowImageChange[5]         = false;
   stateSequence[5]                 = "fire";
   stateScaleAnimation[5]           = false;
   stateSequenceNeverTransition[5]  = true;
   stateSequenceRandomFlash[5]      = true;        // use muzzle flash sequence
   stateScript[5]                   = "onFire";
   stateSound[5]                    = LurkerFireSound;//LurkerFireSoundList;
   stateEmitter[5]                  = GunFireSmokeEmitter;
   stateEmitterTime[5]              = 0.025;

   // Put another round into the chamber if one is available
   stateName[6]                     = "NewRound";
   stateTransitionGeneric0In[6]     = "SprintEnter";
   stateTransitionOnNoAmmo[6]       = "NoAmmo";
   stateTransitionOnTimeout[6]      = "Ready";
   stateWaitForTimeout[6]           = "0";
   stateTimeoutValue[6]             = 0.05;
   stateAllowImageChange[6]         = false;
   stateEjectShell[6]               = true;

   // No ammo in the weapon, just idle until something
   // shows up. Play the dry fire sound if the trigger is
   // pulled.
   stateName[7]                     = "NoAmmo";
   stateTransitionGeneric0In[7]     = "SprintEnter";
   stateTransitionOnMotion[7]       = "NoAmmoMotion";
   stateTransitionOnAmmo[7]         = "ReloadClip";
   stateTimeoutValue[7]             = 0.1;   // Slight pause to allow script to run when trigger is still held down from Fire state
   stateScript[7]                   = "onClipEmpty";
   stateSequence[7]                 = "idle";
   stateScaleAnimation[7]           = false;
   stateScaleAnimationFP[7]         = false;
   stateTransitionOnTriggerDown[7]  = "DryFire";

   stateName[8]                     = "NoAmmoMotion";
   stateTransitionGeneric0In[8]     = "SprintEnter";
   stateTransitionOnNoMotion[8]     = "NoAmmo";
   stateWaitForTimeout[8]           = false;
   stateScaleAnimation[8]           = false;
   stateScaleAnimationFP[8]         = false;
   stateSequenceTransitionIn[8]     = true;
   stateSequenceTransitionOut[8]    = true;
   stateTransitionOnTriggerDown[8]  = "DryFire";
   stateTransitionOnAmmo[8]         = "ReloadClip";
   stateSequence[8]                 = "run";

   // No ammo dry fire
   stateName[9]                     = "DryFire";
   stateTransitionGeneric0In[9]     = "SprintEnter";
   stateTransitionOnAmmo[9]         = "ReloadClip";
   stateWaitForTimeout[9]           = "0";
   stateTimeoutValue[9]             = 0.7;
   stateTransitionOnTimeout[9]      = "NoAmmo";
   stateScript[9]                   = "onDryFire";
   stateSound[9]                    = MachineGunDryFire;

   // Play the reload clip animation
   stateName[10]                     = "ReloadClip";
   stateTransitionGeneric0In[10]     = "SprintEnter";
   stateTransitionOnTimeout[10]      = "Ready";
   stateWaitForTimeout[10]           = true;
   stateTimeoutValue[10]             = 3.0;
   stateReload[10]                   = true;
   stateSequence[10]                 = "reload";
   stateShapeSequence[10]            = "Reload";
   stateScaleShapeSequence[10]       = true;
   stateSound[10]                    = LurkerReloadSound;

   // Start Sprinting
   stateName[11]                    = "SprintEnter";
   stateTransitionGeneric0Out[11]   = "SprintExit";
   stateTransitionOnTimeout[11]     = "Sprinting";
   stateWaitForTimeout[11]          = false;
   stateTimeoutValue[11]            = 0.5;
   stateWaitForTimeout[11]          = false;
   stateScaleAnimation[11]          = false;
   stateScaleAnimationFP[11]        = false;
   stateSequenceTransitionIn[11]    = true;
   stateSequenceTransitionOut[11]   = true;
   stateAllowImageChange[11]        = false;
   stateSequence[11]                = "sprint";

   // Sprinting
   stateName[12]                    = "Sprinting";
   stateTransitionGeneric0Out[12]   = "SprintExit";
   stateWaitForTimeout[12]          = false;
   stateScaleAnimation[12]          = false;
   stateScaleAnimationFP[12]        = false;
   stateSequenceTransitionIn[12]    = true;
   stateSequenceTransitionOut[12]   = true;
   stateAllowImageChange[12]        = false;
   stateSequence[12]                = "sprint";

   // Stop Sprinting
   stateName[13]                    = "SprintExit";
   stateTransitionGeneric0In[13]    = "SprintEnter";
   stateTransitionOnTimeout[13]     = "Ready";
   stateWaitForTimeout[13]          = false;
   stateTimeoutValue[13]            = 0.5;
   stateSequenceTransitionIn[13]    = true;
   stateSequenceTransitionOut[13]   = true;
   stateAllowImageChange[13]        = false;
   stateSequence[13]                = "sprint";
};
