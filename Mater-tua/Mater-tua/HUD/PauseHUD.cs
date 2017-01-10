﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

internal class PauseHud : HUD
{
    private Texture2D _tex;

    public PauseHud()
    {
        _tex = GameEnvironment.getAssetManager().GetSprite("Sprites/HUD/WoodTextureTest");

        _buttons = new List<Button>();
        /* 1 resume game*/
        _buttons.Add(new Button(new Rectangle(96, +160, 192, 64), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/resumeGameButton"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/resumeGameButtonPressed"), true));
        /* 2 settings*/
        _buttons.Add(new Button(new Rectangle(96, +96, 192, 64), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/settingsButton"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/settingsButtonPressed"), true));
        /* 3 quit to menu */
        _buttons.Add(new Button(new Rectangle(96, +32, 192, 64), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/quitToMenuButton"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/quitToMenuButtonPressed"), true));
    }

    public new bool update(InputHelper inputHelper)
    {
        int j = base.update(inputHelper);

        switch (j)
        {
            case 0:
                break;

            case 1:
                Console.WriteLine("Resume game button pressed");
                GameEnvironment.gameStateManager.State = GameStateManager.state.Playing;
                break;

            case 2:
                Console.WriteLine("Settings button pressed");
                GameEnvironment.gameStateManager.State = GameStateManager.state.PauseSettings;
                break;

            case 3:
                Console.WriteLine("quit to menu button pressed");
                GameEnvironment.gameStateManager.State = GameStateManager.state.Menu;
                break;
        }
        return false;
    }

    public override void draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i + _tex.Width <= GameEnvironment.getCamera().getScreenSize().X + _tex.Width; i += _tex.Width) // repeats the HUD texture till edge of screen
            spriteBatch.Draw(_tex, new Rectangle(i, (int)GameEnvironment.getCamera().getScreenSize().Y - _tex.Height, _tex.Width, _tex.Height), Color.White);

        foreach (Button b in _buttons)
        {
            b.draw(spriteBatch);
        }
    }
}