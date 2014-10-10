using Asteroid.Tools;
using Battleship.Entity;
using Battleship.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Managers
{
    public class ShootManager
    {
        private List<Missile> bombs;
        private List<Point> bombPositions;
        private List<Particle> particles;
        private ShipField field;
        private bool isShooting;

        public ShootManager(ShipField field)
        {
            this.isShooting = false;
            this.bombs = new List<Missile>();
            this.bombPositions = new List<Point>();
            this.particles = new List<Particle>();
            this.field = field;
        }

        public bool update(float delta, ShipField target)
        {
            for (int i = 0; i < bombs.Count; i++)
            {
                Missile missile = bombs[i];
                missile.update(delta);
                if (!missile.isMissileAlive())
                {
                    bombs.Remove(missile);
                }
            }

            if (isShooting)
            {
                if (bombs.Count == 0)
                {
                    finishShooting(target);
                }
                return false;
            }
            else
                return true;
        }

        public void updateParticles(float delta)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                Particle particle = particles[i];
                particle.update(delta);
                if (particle.getPosition().Y > 720 || !particle.isAlive())
                {
                    particles.Remove(particle);
                }
            }
        }

        public void draw(SpriteBatch batch)
        {
            foreach (var item in bombs)
            {
                item.draw(batch);
            }

            foreach (var particle in particles)
            {
                particle.draw(batch);
            }
        }

        public bool shoot(ShipField target, float x, float y)
        {
            if (field.getSelectedAttack() == "Normal Strike")
            {
                if (isTargetValid(target, x, y))
                {
                    field.consumeAttack();
                    return true;
                }
            }
            else if (field.getSelectedAttack() == "Horizontal Strike")
            {
                bool hit = false;
                for (int i = 0; i < World.FIELD_SIZE; i++)
                {
                    if (isTargetValid(target, World.TILE_SIZE / 2 + target.getBounds().X + (i * World.TILE_SIZE), y))
                    {
                        hit = true;
                    }
                }
                if (hit)
                {
                    field.consumeAttack();
                    return true;
                }
            }
            else if (field.getSelectedAttack() == "Vertical Strike")
            {
                bool hit = false;
                for (int i = 0; i < World.FIELD_SIZE; i++)
                {
                    if (isTargetValid(target, x, World.TILE_SIZE / 2 + target.getBounds().Y + i * World.TILE_SIZE)) hit = true;
                }
                if (hit)
                {
                    field.consumeAttack();
                    return true;
                }
            }
            else if (field.getSelectedAttack() == "Circle Strike")
            {
                bool hit = false;
                if (isTargetValid(target, x, y + World.TILE_SIZE)) hit = true;
                if (isTargetValid(target, x, y - World.TILE_SIZE)) hit = true;
                if (isTargetValid(target, x + World.TILE_SIZE, y)) hit = true;
                if (isTargetValid(target, x - World.TILE_SIZE, y)) hit = true;

                if (hit)
                {
                    field.consumeAttack();
                    return true;
                }
            }
            else if (field.getSelectedAttack() == "Nuke")
            {
                for (int i = 0; i < World.FIELD_SIZE; i++)
                {
                    for (int j = 0; j < World.FIELD_SIZE; j++)
                    {
                        isTargetValid(target, World.TILE_SIZE / 2 + target.getBounds().X + (i * World.TILE_SIZE), World.TILE_SIZE / 2 + target.getBounds().Y + j * World.TILE_SIZE);
                    }
                }
                return true;
            }
            return false;

        }

        private void finishShooting(ShipField target)
        {
            particles.Clear();

            bool hit = false;
            foreach (var item in bombPositions)
            {
                if (target.hit(item.X, item.Y))
                {
                    hit = true;
                    spawnParticles(item.X, item.Y);
                }
            }

            if (Assets.SOUND)
            {
                if (hit)
                    Assets.bombSound.Play(.45f, 0, 0);
                else
                    Assets.missSound.Play(.3f, 0, 0);
            }

            bombPositions.Clear();
            bombs.Clear();
            isShooting = false;
        }

        private void spawnParticles(int x, int y)
        {
            for (int i = 0; i < 100; i++)
            {
                Particle p = new Particle(x, y, MathUtils.random(-100 * MathUtils.random(), 100), MathUtils.random(-100, 100 * MathUtils.random()), 3, Color.Red);
                p.setLife(.2f);
                //particles.Add(p); // todo enable in future
            }
        }

        public bool isTargetValid(ShipField target, float x, float y)
        {
            Tile tile = target.getTile(x, y);
            if (tile != null)
            {
                int tileId = tile.getId();
                if (tile.isHitable())
                {
                    isShooting = true;
                    addBomb(x, y);
                    return true;
                }
            }
            return false;
        }

        private void addBomb(float x, float y)
        {
            bombPositions.Add(new Point((int)x, (int)y));
            Missile missile = new Missile(x, y);
            bombs.Add(missile);
        }

        public List<Point> getBombPositions()
        {
            return bombPositions;
        }
    }
}
