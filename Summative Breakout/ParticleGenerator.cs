using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Summative_Breakout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Generator
{
    public class ParticleEngine
    {
        private Random _generator;
        private Vector2 _emitterLocation { get; set; }
        private Vector2 _emitterDirection;
        private float _gravity;
        private List<Particle> _particles;
        private List<Texture2D> _textures;
        private bool _enabled;
        private bool _gravityEnabled;

        public ParticleEngine(List<Texture2D> textures, Vector2 location, bool enabled, bool gravityEnabled)
        {
            _emitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _generator = new Random();
            _enabled = enabled;
            _gravityEnabled = gravityEnabled;
            _emitterDirection = Vector2.Zero;
        }
        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[_generator.Next(_textures.Count)];
            Vector2 position = _emitterLocation;
            Vector2 velocity = new Vector2(_emitterDirection.X, (float)_generator.NextDouble() * 2 - 1);
            float angle = 0;
            float angularVelocity = 0.1f * (float)(_generator.NextDouble() * 2 - 1);
            Color color = new Color(
            (float)_generator.NextDouble(),
            (float)_generator.NextDouble(),
            (float)_generator.NextDouble());
            int size = _generator.Next(10, 20);
            int ttl = 1 + _generator.Next(20);
            return new Particle(texture, position, velocity, angle, angularVelocity, Color.White, size, ttl, _gravityEnabled);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in _particles)
                particle.Draw(spriteBatch);
        }
        public void Update()
        {
            if (_enabled)
            {
                int total = 3;
                for (int i = 0; i < total; i++)
                {
                    _particles.Add(GenerateNewParticle());
                }
            }
            for (int i = 0; i < _particles.Count; i++)
            {
                _particles[i].Update();
                if (_particles[i].Dead)
                {
                    _particles.RemoveAt(i);
                    i--;
                }
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }
        public Vector2 EmitterLocation
        {
            get { return _emitterLocation; }
            set { _emitterLocation = value; }
        }
        public Vector2 EmitterDirection
        {
            get { return _emitterDirection; }
            set { _emitterDirection = value; }
        }
    }
}
