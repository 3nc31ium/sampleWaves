using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Camera
{
    //Move speed for camera
    public Vector3 cameraPosition;
    public Matrix view;
    public Matrix projection;
    public Matrix world;
    public GraphicsDevice _device;
    public Vector3 cameraForward;
    public Vector3 target;

    public float _moveScale = 1;

    private float speed;
    public Matrix cameraRotation;

    public Camera(GraphicsDevice device, float farDistance)
    {
      
        _device = device;

        cameraPosition = new Vector3(0, 0, 500);
        target = new Vector3(0, 0, 0);


        speed = .2f;

        cameraRotation = Matrix.Identity;
       

        world = Matrix.CreateTranslation(0, 0, 0);
        view = Matrix.CreateLookAt(cameraPosition, target, new Vector3(0, 1, 0));
        projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _device.Viewport.AspectRatio, 2.0f, farDistance);



    }

    private void MoveCamera(Vector3 addedVector)
    {
        cameraPosition += speed * addedVector;
    }

  
    public void UpdateCamera(Vector3 playerPosition,Matrix playerRoation)
    {


        view = Matrix.CreateLookAt(cameraPosition+playerPosition, playerPosition, new Vector3(0, 1, 0));

    
    }

}