﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Particles;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Timers;

namespace Whitespace.App
{
    internal class Orb : PhysicsObject
    {
        private ParticleEffect _destroyedParticles;

        private ParticleEffect _idleParticles;

        public Orb(Texture2D texture, Texture2D particleTexture, Color tint) : base(texture)
        {
            Tint = tint;
            _destroyedParticles = new ParticleEffect(autoTrigger: false)
            {
                Emitters = new List<ParticleEmitter>
                {
                    new ParticleEmitter(
                        new TextureRegion2D(particleTexture),
                        5, TimeSpan.FromSeconds(0.5d), Profile.Point())
                    {
                        AutoTrigger = false,
                        Parameters = new ParticleReleaseParameters()
                        {
                            Speed = new(500f, 1000f),
                            Rotation = new (0f, MathHelper.TwoPi),
                            Opacity = 1f,
                            Color = tint.ToHsl()
                        },
                        Modifiers =
                        {
                            new AgeModifier
                            {
                                Interpolators =
                                {
                                    new ScaleInterpolator()
                                    {
                                        StartValue = new Vector2(100f),
                                        EndValue = Vector2.Zero
                                    }
                                }
                            },
                            new RotationModifier() { RotationRate = 1f },
                            new LinearGravityModifier()
                            {
                                Direction = Vector2.UnitY,
                                Strength = 10000f
                            },


                        },
                        
                    }
                }
            };


        }

        public void Destroy()
        {
            _destroyedParticles.Position = Position;
            for (int i = 0; i < 5; i++)
            {
                _destroyedParticles.Trigger();
            }
        }

        public override void Update(float timeSpeed)
        {
            base.Update(timeSpeed);
            _destroyedParticles.Update(timeSpeed);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(_destroyedParticles);
        }
    }
}
