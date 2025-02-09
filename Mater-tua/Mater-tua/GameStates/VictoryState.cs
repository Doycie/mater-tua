﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal class VictoryState : GameState
{
    private CustomCursor _customCursor;
    private MouseState _mouseState;
    private Vector2 _lastMousePos;
    private Vector2 _currentMousePos;
    private bool _mouseReleased;
    private HUD _hud;

    public VictoryState()
    {
        _customCursor = new CustomCursor();
        _mouseState = Mouse.GetState();
        _hud = new VictoryHud();
    }

    public void update(GameTime gameTime)
    {
    }

    public void drawHUD(SpriteBatch spriteBatch)
    {
        _hud.draw(spriteBatch);
        _customCursor.draw(spriteBatch);
    }

    public void draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public void handleInput(InputHelper inputHelper)
    {
        _customCursor.updateCursorPosition(inputHelper);
        _mouseState = Mouse.GetState();

        _currentMousePos = _customCursor.getMousePos();

        if (inputHelper.KeyPressed(Keys.Escape))
        {
            GameEnvironment.gameStateManager.State = GameStateManager.state.Menu;
        }

        if ((_hud as VictoryHud).update(inputHelper))
            GameEnvironment.gameStateManager.State = GameStateManager.state.Playing;

        if (inputHelper.MouseLeftButtonDown())
        {
            if (_mouseReleased == false)
            {
                _mouseReleased = true;
                _lastMousePos = _customCursor.getMousePos();
            }
        }

        if (inputHelper.KeyPressed(Keys.Enter))
        {
            GameEnvironment.gameStateManager.State = GameStateManager.state.Menu;
        }
    }
}
