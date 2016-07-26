using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Capstone
{

    class Particle
    {
        public float timeOfBirth;
        public ParticleVertex[] verticies;
        public Vector3 Position;
        public Particle(Vector3 _position,float gameTime)
        {

            timeOfBirth = gameTime;
            Position = _position;
            verticies = new ParticleVertex[4];
            verticies[0] = new ParticleVertex(_position, new Vector2(0, 0), timeOfBirth);
            verticies[1] = new ParticleVertex(new Vector3(_position.X,_position.Y + 1,_position.Z), new Vector2(0, 1), timeOfBirth);
            verticies[2] = new ParticleVertex(new Vector3(_position.X + 1, _position.Y + 1, _position.Z), new Vector2(1, 1), timeOfBirth);
            verticies[3] = new ParticleVertex(new Vector3(_position.X + 1, _position.Y , _position.Z), new Vector2(1, 0), timeOfBirth);
            
        }

       
    }
}

