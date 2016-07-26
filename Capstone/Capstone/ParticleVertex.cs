using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public struct ParticleVertex
{
    public Vector3 Position;
    public Vector2 TextureCoordinate;
    public float time;
    public static int SizeInBytes = (3 + 2 + 4 + 1) * sizeof(float);

    public static VertexElement[] VertexElements = new VertexElement[]
     {
         new VertexElement( 0, VertexElementFormat.Vector3,  VertexElementUsage.Position, 0 ),
         new VertexElement( sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0 ),
         new VertexElement( sizeof(float) * 5, VertexElementFormat.Single,  VertexElementUsage.TextureCoordinate, 1), 
     };

    public ParticleVertex(Vector3 _position, Vector2 _texcoord,float _time)
    {
    Position=_position;
    TextureCoordinate=_texcoord;
    time = _time;

    }

    public ParticleVertex(ParticleVertex vertex)
    {
        Position = vertex.Position;
        TextureCoordinate = vertex.TextureCoordinate;
        time = vertex.time;
    }
}


