﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Entity
{

    protected Vector2 _position;
    protected int _layer;
    

    public Entity(int layer = 0)
    {
        this._layer = layer;
        Reset();
    }

    public virtual void Update()
    {

    }
    
    public virtual void Reset()
    {
        _position = Vector2.Zero;
    }

    public virtual Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public virtual int Layer
    {
        get { return _layer; }
        set { _layer = value; }
    }

}


