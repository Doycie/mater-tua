﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

internal class SettingsState : GameState
{
    private CustomCursor _customCursor;
    private MouseState _mouseState;
    private Vector2 _lastMousePos;
    private Vector2 _currentMousePos;
    private bool _mouseReleased;
    private HUD _hud;

    public SettingsState()
    {
        _customCursor = new CustomCursor();
        _mouseState = Mouse.GetState();
        _hud = new SettingsHud();
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

        if ((_hud as SettingsHud).update(inputHelper))
            GameEnvironment.gameStateManager.State = GameStateManager.state.Playing;

        if (inputHelper.MouseLeftButtonDown())
        {
            if (_mouseReleased == false)
            {
                _mouseReleased = true;
                _lastMousePos = _customCursor.getMousePos();
            }
        }

        if (inputHelper.KeyPressed(Keys.Escape))
        {
            GameEnvironment.gameStateManager.State = GameStateManager.state.Menu;
        }

        if (inputHelper.KeyPressed(Keys.Back))
        {
            GameEnvironment.gameStateManager.State = GameStateManager.state.Menu;
        }
    }
}