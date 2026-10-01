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
        }
        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[_generator.Next(_textures.Count)];
            Vector2 position = _emitterLocation;
            Vector2 velocity = new Vector2(1f * (float)(_generator.NextDouble() * 2 - 1), 1f * (float)(_generator.NextDouble() * 2 - 1));
            //Vector2 velocity = new Vector2(0, 0);
            float angle = 0;
            float angularVelocity = 0.1f * (float)(_generator.NextDouble() * 2 - 1);
            Color color = new Color(
            (float)_generator.NextDouble(),
            (float)_generator.NextDouble(),
            (float)_generator.NextDouble());
            int size = _generator.Next(5, 10);
            int ttl = 100 + _generator.Next(40);
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl, _gravityEnabled);
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
                int total = 5;
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
    }
}