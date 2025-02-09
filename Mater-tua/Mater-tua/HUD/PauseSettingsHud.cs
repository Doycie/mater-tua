﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

internal class PauseSettingsHud : HUD
{
    private Texture2D _tex;

    public PauseSettingsHud()
    {
        _tex = GameEnvironment.getAssetManager().GetSprite("Sprites/HUD/WoodTextureTest");

        _buttons = new List<Button>();
        /* 1 fullscreen*/
        _buttons.Add(new Button(new Rectangle(96, 190, 250, 130), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/fullscreenButton"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/fullscreenButtonPressed"), true));
        /* 2 back*/
        _buttons.Add(new Button(new Rectangle(96, -38, 240, 110), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/backButton"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/backButtonPressed"), true));
        /* 3 volume up */
        _buttons.Add(new Button(new Rectangle(-70, 60, 78, 78), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeUp"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeUpPressed"), true));
        /* 4 volume down */
        _buttons.Add(new Button(new Rectangle(11, 60, 76, 76), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeDown"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeDownPressed"), true));
        /* 5 volume mute */
        _buttons.Add(new Button(new Rectangle(94, 60, 76, 76), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeMute"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeMutePressed"), true));
    }

    public new bool update(InputHelper inputHelper)
    {
        int j = base.update(inputHelper);

        switch (j)
        {
            case 0:
                break;

            case 1:
                Console.WriteLine("Fullscreen pressed");
                GameEnvironment.graphics.ToggleFullScreen();
                break;

            case 2:
                Console.WriteLine("Back pressed");
                GameEnvironment.gameStateManager.State = GameStateManager.state.Pause;
                break;

            case 3:
                MediaPlayer.Volume += (float)0.1;
                try
                {
                    SoundEffect.MasterVolume += (float)0.1;
                }
                catch
                {
                    SoundEffect.MasterVolume = 1;
                }
                Console.WriteLine(SoundEffect.MasterVolume);
                break;

            case 4:
                MediaPlayer.Volume -= (float)0.1;
                try
                {
                    SoundEffect.MasterVolume -= (float)0.1;
                }
                catch
                {
                    SoundEffect.MasterVolume = 0;
                }
                Console.WriteLine(SoundEffect.MasterVolume);
                break;

            case 5:
                MediaPlayer.Volume = (float)0.0;
                SoundEffect.MasterVolume = (float)0.0;
                break;
        }
        return false;
    }

    public override void draw(SpriteBatch spriteBatch)
    {
        
        foreach (Button b in _buttons)
        {
            b.draw(spriteBatch);
        }
    }
}