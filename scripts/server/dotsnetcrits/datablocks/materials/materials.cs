singleton Material(floor)
{
   mapTo = "unmapped_mat";
   diffuseMap[0] = "art/shapes/dotsnetcrits/levels/garage/floorGrey.png";
   diffuseColor[0] = "0.835294 0.788235 0.286275 1";
   diffuseColor[1] = "1 1 1 1";
};

singleton Material(floor0)
{
   mapTo = "unmapped_mat";
   diffuseColor[1] = "White";
   diffuseMap[0] = "art/shapes/dotsnetcrits/levels/garage/floor0.png";
};

singleton Material(archedRoomConnector)
{
   mapTo = "unmapped_mat";
   diffuseMap[0] = "art/shapes/dotsnetcrits/levels/garage/archedRoomConnector.png";
};

singleton Material(face_mat)
{
   mapTo = "face";
   diffuseMap[0] = "art/shapes/dotsnetcrits/actors/quechan/face_01.png";
   specular[3] = "1 1 1 1";
};

singleton CubemapData( cyberpunk0Cubemap )
{
   cubeFace[0] = "art/shapes/dotsnetcrits/levels/cyberpunk0/sky/left";
   cubeFace[1] = "art/shapes/dotsnetcrits/levels/cyberpunk0/sky/back";
   cubeFace[2] = "art/shapes/dotsnetcrits/levels/cyberpunk0/sky/right";
   cubeFace[3] = "art/shapes/dotsnetcrits/levels/cyberpunk0/sky/forward";
   cubeFace[4] = "art/shapes/dotsnetcrits/levels/cyberpunk0/sky/up";
   cubeFace[5] = "art/shapes/dotsnetcrits/levels/cyberpunk0/sky/down";
};

singleton Material( cyberpunk0SkyMat )
{
   cubemap = cyberpunk0Cubemap;
   materialTag0 = "Skies";
   isSky = true;
};
