using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public class GameObject
{
    protected Vector3 position = Vector3.Zero;
    protected Vector3 rotation = Vector3.Zero;
    protected float scale = 1.0f;

    protected Model model = null;
    protected bool visible = true;

    public Vector3 xSpeed;
    public Vector3 zSpeed;

    public GameObject(Vector3 _position, Model _model)
    {
        position = _position;
        model = _model;
    }

    public Vector3 getPosition()
    {
        return position;
    }

    public void setPosition(Vector3 _position)
    {
        position = _position;
    }

    public void updatePosition(Vector3 movement)
    {
        position += movement;
    }

    public void setSpeed(float speed)
    {
        xSpeed = new Vector3(speed, 0, 0);
        zSpeed = new Vector3(0, 0, speed);
    }

    public bool getVisible()
    {
        return visible;
    }

    public void setVisible(bool _visible)
    {
        visible = _visible;
    }

    public void Draw(Matrix projection, Matrix view)
    {
        if (visible)
        {
            Matrix world = Matrix.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z) *
            Matrix.CreateScale(scale) *
            Matrix.CreateTranslation(position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = world;
                    effect.Projection = projection;
                    effect.View = view;
                }
                mesh.Draw();
            }
        }

        //end of draw
    }

    //end class
}
