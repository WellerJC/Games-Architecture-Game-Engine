using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using OpenGL_Game.Level;
using System.Drawing;
using System;
using OpenTK.Audio.OpenAL;
using OpenGL_Game.Menus;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        public static float dt = 0;
        EntityManager entityManager;
        SystemManager systemManager;
        CollisionManager collisionManager;
        KeyboardKeyEventArgs e;
        int playerHealth = 3;
        int alphaHealth = 3; int betaHealth = 3; int charlieHealth = 3;
        Drone Alpha, Beta, Charlie; Bullet initialBullet, upgradedBullet;
        Health healthUpgrade; Weapon weaponUpgrade;
        ComponentPosition aP, bP, cP, f1P, f2P, hP, wP, pP, upP;
        ComponentCollisionSphere aC, bC, cC, f1C, f2C, hC, wC, pC;
        ComponentVelocity f1V, f2V, aV;
        bool upgradeWeapon = false;
        bool keyBinds; public static bool debugmode;
        bool enemyDisabled = true;
        int wallCollisions = 0; int enemyTimeout = 0;
        Player player, upgradedPlayer;
        Vector3 previousPos;
        bool[] keysPressed = new bool[255];
        public Camera camera;
        cubeMap cmap;
        public static bool closegame = false; public static bool returntomenu = false;
        public static GameScene gameInstance;

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            collisionManager = new CollisionManager();
            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            // Set Camera
            camera = new Camera(new Vector3(0, 0.3f, 7), new Vector3(0, 0.3f, 0), (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);
            cmap = new cubeMap(20, 20, "Geometry/Map/cubemap.txt");
            keyBinds = false;

            // level = new cubeMap(20); level.LoadMap("Geometry/Map/cubeMap.txt");
            CreateEntities();
            CreateSystems();

            if(debugmode == null)
            {
                debugmode = false;
            }
            // TODO: Add your initialization logic here

        }

        private void CreateEntities()
        {
            CreateMap();
            // AddComponents(entity, position, velocity, model, audio, radius);
            Alpha = new Drone("Alpha"); AddComponents(Alpha, new Vector3(-5.0f, -1f, 0.8f), new Vector3(0f, 0f, 0f), "Geometry/Enemy/enemy.obj", "Audio/buzz.wav", 0.8f);
            Beta = new Drone("Beta"); AddComponents(Beta, new Vector3(0f, -1f, 0.0f), new Vector3(0f, 0f, 0f), "Geometry/Enemy/enemy.obj", "Audio/buzz.wav", 0.8f);
            Charlie = new Drone("Charlie"); AddComponents(Charlie, new Vector3(5.0f, -1f, 0.0f), new Vector3(0f, 0f, 0f), "Geometry/Enemy/enemy.obj", "Audio/buzz.wav", 0.8f);

            initialBullet = new Bullet("Bullet"); AddComponents(initialBullet, new Vector3(-1000f, -1000f, -1000f), new Vector3(0f, 0f, 0f), "Geometry/Bullet/bullet.obj", "Audio/weapon_fire.wav", 0.5f);
            upgradedBullet = new Bullet("upgradedBullet"); AddComponents(upgradedBullet, new Vector3(-1000f, -1000f, -1000f), new Vector3(0f, 0f, 0f), "Geometry/Bullet/bullet.obj", "Audio/weapon_fire.wav", 0.5f);

            healthUpgrade = new Health("healthUpgrade"); AddComponents(healthUpgrade, new Vector3(2.5f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), "Geometry/Health_Upgrade/health.obj", "Audio/enemy_hum.wav", 0.5f);
            weaponUpgrade = new Weapon("weaponUpgrade"); AddComponents(weaponUpgrade, new Vector3(-2.5f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), "Geometry/Weapon_Upgrade/weaponUpgrade.obj", "Audio/enemy_hum.wav", 0.5f);

            player = new Player("Player"); AddComponents(player, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(2.0f, 2.0f, 2.0f), "Geometry/gun/pistol.obj", "Audio/game_music.wav", 1.0f);
            upgradedPlayer = new Player("upgradedPlayer"); AddComponents(upgradedPlayer, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), "Geometry/Skybox/skybox.obj", "Audio/low_health.wav", 1.0f);
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemPhysics();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemAudio();
            systemManager.AddSystem(newSystem);
            newSystem = new SystemCollision();
            systemManager.AddSystem(newSystem);
        }

        private void CreateMap()
        {
            Entity map = new Entity("Map"); map.AddComponent(new ComponentPosition(0, -1, 0));
            map.AddComponent(new ComponentGeometry("Geometry/Map/cubemap.obj")); map.AddComponent(new ComponentShaderDefault());
            map.AddComponent(new ComponentCollisionMesh(cmap)); entityManager.AddEntity(map);
        }

        public override void Update(FrameEventArgs e)
        {

            dt = (float)e.Time;
            //System.Console.WriteLine("fps=" + (int)(1.0/dt));

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            if (closegame == true) { sceneManager.Exit();  }
            // TODO: Add your update logic here
            if (keyBinds == false)
            {
                if (keysPressed[(char)Key.W]) camera.MoveForward(0.025f); if (keysPressed[(char)Key.S]) camera.MoveForward(-0.025f);

                if (keysPressed[(char)Key.A]) camera.RotateY(-0.01f); if (keysPressed[(char)Key.D]) camera.RotateY(0.01f);

                if (keysPressed[(char)Key.Space]) CreateBullet(); if (keysPressed[(char)Key.P]) { PauseMenu pM = new PauseMenu(); pM.Show(); }
            }

            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);

            #region GetVariables
            aP = GetPosition(Alpha); aC = GetCollision(Alpha); bP = GetPosition(Beta); bC = GetCollision(Beta);
            cP = GetPosition(Charlie); cC = GetCollision(Charlie); pP = GetPosition(player); pC = GetCollision(player);
            upP = GetPosition(upgradedPlayer); hP = GetPosition(healthUpgrade); hC = GetCollision(healthUpgrade);
            wP = GetPosition(weaponUpgrade); wC = GetCollision(weaponUpgrade); aV = GetVelocity(Alpha);
            f1P = GetPosition(initialBullet); f1C = GetCollision(initialBullet); f1V = GetVelocity(initialBullet);
            f2P = GetPosition(upgradedBullet); f2C = GetCollision(upgradedBullet); f2V = GetVelocity(upgradedBullet);
            #endregion

            UpdatePlayerPosition();
            if (enemyDisabled == false)  {  UpdateEnemyPosition(); }
            if (enemyDisabled == true)
            {
                if(debugmode == false)
                {
                    enemyTimeout++;
                    Console.WriteLine(enemyTimeout);
                    if(enemyTimeout > 400) { enemyTimeout = 0; enemyDisabled = false; }
                }
            }
            //PositionWriter((ComponentCollision)enemyPositionComponent, (ComponentCollision)bulletPositionComponent, (ComponentCollision)playerPositionComponent);
            if(debugmode == true)
            {
                Console.WriteLine("Debug Mode on");
            }
            ObjectsCollide();
            previousPos = pP.Position;
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);
            // Render score
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.clearColour = Color.Transparent;
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), "  Alpha health left: " + alphaHealth + "  Beta health left: " + betaHealth + "  Charlie health: " + charlieHealth, 18, StringAlignment.Near, Color.White);
            GUI.Label(new Rectangle(0, 40, (int)width, (int)(fontSize * 2f)), " Wall Collisions: " + wallCollisions, 18, StringAlignment.Near, Color.White);
            GUI.Label(new Rectangle(0,650, (int)width, (int)(fontSize * 2f)), "Player Health: " + playerHealth, 18, StringAlignment.Near, Color.White);
            GUI.Render();
        }

        public override void Close()
        {
            keyBinds = true;
            ResourceManager.RemoveAllAssets();
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;

        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = true;
        }

        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keysPressed[(char)e.Key] = false;
        }

        public void UpdatePlayerPosition()
        {
            pP.Position = camera.cameraPosition;
            if (upgradeWeapon == true) { upP.Position = camera.cameraPosition; }
        }

        public void CreateBullet()
        {
            f1P.Position = camera.cameraPosition + new Vector3(0.5f, -0.2f, 0); f1V.Velocity = 10 * camera.cameraDirection;
            if (upgradeWeapon == true)
            { f2P.Position = camera.cameraPosition + new Vector3(-0.5f, -0.2f, 0); f2V.Velocity = 10 * camera.cameraDirection; }
        }

        public void PlayerHit()
        { 
            if (playerHealth == 1) { GameLost gl = new GameLost(); gl.Show(); } playerHealth--;
            camera.cameraPosition = new Vector3(0, 0.3f, 7);
        }

        public void EnemyHit(string enemyType, ComponentPosition position)
        {
            if (enemyType == "Alpha") { if (alphaHealth == 1) { PlaySound("Audio/death_violent_long1.wav", position); position.Position = new Vector3(1000, 1000, 1000); } alphaHealth--; }
            else if (enemyType == "Beta") { if (betaHealth == 1) { PlaySound("Audio/death_violent_long1.wav", position); position.Position = new Vector3(1000, 1000, 1000); } betaHealth--; }
            else if (enemyType == "Charlie") { if (charlieHealth == 1) { PlaySound("Audio/death_violent_long1.wav", position); position.Position = new Vector3(1000, 1000, 1000); } charlieHealth--; }

            if (alphaHealth == 0 && betaHealth == 0 && charlieHealth == 0) { GameWon gw = new GameWon(); gw.Show(); }
        }

        public void HealthPickup(ComponentPosition player, ComponentPosition health)
        {
            PlaySound("Audio/item_get_1up.wav", player); health.Position = new Vector3(-1000, -1000, -1000); playerHealth++;
        }

        public void WeaponPickup(ComponentPosition player, ComponentPosition weaponUpgrade)
        {
            PlaySound("Audio/item_slot_start.wav", player); weaponUpgrade.Position = new Vector3(-1000, -1000, -1000); upgradeWeapon = true;

        }

        public void DroneDisabler() 
        {
            enemyDisabled = true;
        }

        public void BulletDelete(ComponentPosition bullet, ComponentPosition bullet2)
        {
            bullet.Position = new Vector3(-1000, -1000, -1000); bullet2.Position = new Vector3(-1000, -1000, -1000);
        }

        public IComponent GetComponent(Entity name, ComponentTypes type)
        {
            IComponent variable = name.Components.Find(delegate (IComponent component) { return component.ComponentType == type; }); return variable;
        }

        public void ObjectsCollide()
        {
            if (collisionManager.SphereCollision(pC, aC, pP, aP) == true || collisionManager.SphereCollision(pC, bC, pP, bP) == true ||
                collisionManager.SphereCollision(pC, cC, pP, cP) == true) { PlayerHit(); }
            if (collisionManager.SphereCollision(bC, f1C, bP, f1P) == true || collisionManager.SphereCollision(bC, f2C, bP, f2P) == true)
            { EnemyHit("Beta", bP); BulletDelete(f1P, f2P); }
            if (collisionManager.SphereCollision(aC, f1C, aP, f1P) == true || collisionManager.SphereCollision(aC, f2C, aP, f2P) == true)
            { EnemyHit("Alpha", aP); BulletDelete(f1P, f2P); }
            if (collisionManager.SphereCollision(cC, f1C, cP, f1P) == true || collisionManager.SphereCollision(cC, f2C, cP, f2P) == true)
            { EnemyHit("Charlie", cP); BulletDelete(f1P, f2P); }
            if (collisionManager.SphereCollision(hC, pC, hP, pP) == true) { HealthPickup(pP, hP); }
            if (collisionManager.SphereCollision(pC, wC, pP, wP) == true) { WeaponPickup(pP, wP);}

            foreach(ComponentCollisionLine line in cmap.collisionMesh)
            {
                
               if (collisionManager.LineCollision(pP, line, previousPos) == true) { wallCollisions++; PlaySound("Audio/item_get_1up.wav", pP); } ;
            }
        }

        public void AddComponents(Entity entity, Vector3 pos, Vector3 vel, string geo, string audio, float radius)
        {
            entity.AddComponent(new ComponentPosition(pos.X, pos.Y, pos.Z)); entity.AddComponent(new ComponentVelocity(vel)); entity.AddComponent(new ComponentGeometry(geo));
            entity.AddComponent(new ComponentAudio(pos.X, pos.Y, pos.Z, audio)); entity.AddComponent(new ComponentCollisionSphere(true, radius)); entity.AddComponent(new ComponentShaderDefault());
            entityManager.AddEntity(entity);
        }

        public void PlaySound(string audioName, ComponentPosition player)
        {
            int audioBuffer = ResourceManager.LoadAudio(audioName);
            int audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            Vector3 sourcePosition = new Vector3(player.Position);
            AL.Source(audioSource, ALSource3f.Position, ref sourcePosition);
            AL.SourcePlay(audioSource); // play the ausio source
           
        }

        public ComponentPosition GetPosition(Entity name)
        {
            IComponent p1; ComponentPosition position; p1 = GetComponent(name, ComponentTypes.COMPONENT_POSITION);
            position = (ComponentPosition)p1; return position;
        }

        public ComponentCollisionSphere GetCollision(Entity name)
        {
            IComponent c1; ComponentCollisionSphere collision; c1 = GetComponent(name, ComponentTypes.COMPONENT_COLLISION_SPHERE);
            collision = (ComponentCollisionSphere)c1; return collision;
        }

        public ComponentVelocity GetVelocity(Entity name)
        {
            IComponent v1; ComponentVelocity velocity; v1 = GetComponent(name, ComponentTypes.COMPONENT_VELOCITY);
            velocity = (ComponentVelocity)v1; return velocity;
        }

        public void UpdateEnemyPosition()
        {
            aV.Velocity = camera.cameraPosition;
        }

    }
}
