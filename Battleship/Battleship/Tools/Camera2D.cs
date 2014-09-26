using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Tools
{
    /**
     * Camera makes the live a lot easier by translating a matrix and then tell the spritebatch to translate all objects by the values
     * set in this class
     */
    public class Camera2D
    {
        private float rotation;

        private Matrix transform;
        private Vector2 position;
        private Vector2 zoom;
        private GraphicsDevice graphicsDevice;
        private Viewport defaultViewPort;

        public Camera2D(GraphicsDevice graphicsDevice, Viewport defaultViewPort)
        {
            this.graphicsDevice = graphicsDevice;
            this.defaultViewPort = defaultViewPort;
            this.rotation = 0f;
            this.zoom = new Vector2(1, 1);
            this.position = Vector2.Zero;
        }

        public void update(float delta)
        {
            zoom.X = graphicsDevice.Viewport.Width / (float)defaultViewPort.Width;
            zoom.Y = graphicsDevice.Viewport.Height / (float)defaultViewPort.Height;
        }

        // Gets the matrix used by the spritebatch
        public Matrix getMatrix()
        {
            transform =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom.X, zoom.Y, 1)) *
                Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
           
            return transform;
        }

        public Vector2 getZoom()
        {
            return zoom;
        }

        public void setZoom(float width, float height)
        {
            this.zoom.X = width;
            this.zoom.Y = height;
        }

        public float getRotation()
        {
            return rotation;
        }

        public void setRotation(float rot)
        {
            rotation = rot;
        }

        public void Move(Vector2 amount)
        {
            position += amount;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(float x, float y)
        {
            position.X = x;
            position.Y = y;
        }

        public void setPosition(Vector2 pos)
        {
            position.X = pos.X;
            position.Y = pos.Y;
        }
    }
}
