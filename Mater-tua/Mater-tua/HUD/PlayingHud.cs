﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

internal class PlayingHud : HUD
{
    private Minimap _minimap;

    public PlayingHud()
    {
        _minimap = new Minimap(256);
        _buttons = new List<Button>();
        _resources = new List<Resources>();
        ///* 1 */
        //_buttons.Add(new Button(new Rectangle(32, 114, 100, 100), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/Button"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/ButtonPressed"), false));
        ///* 2 */
        //_buttons.Add(new Button(new Rectangle(168, 114, 100, 100), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/Button"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/ButtonPressed"), false));
        ///* 3 */
        //_buttons.Add(new Button(new Rectangle(296, 114, 100, 100), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/Button"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/ButtonPressed"), false));
        ///* 4 */
        //_buttons.Add(new Button(new Rectangle(432, 114, 40, 40), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeDown"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeDownPressed"), false));
        ///* 5 */
        //_buttons.Add(new Button(new Rectangle(432, 57, 40, 40), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeUp"), GameEnvironment.getAssetManager().GetSprite("Sprites/Buttons/VolumeUpPressed"), false));

        _resources.Add(new Resources(1));
        _resources.Add(new Resources(2));
        _resources.Add(new Resources(3));
    }

    public override void draw(SpriteBatch s)
    {
        base.draw(s);

        _minimap.draw(s);
    }

    public void update(InputHelper inputHelper, List<Entity> selectedEntities, Level level)
    {
        int j = base.update(inputHelper);

        _minimap.update(level);

        switch (j)
        {
            case 0:
                break;

            case 1:
                foreach (BasicMeleeUnit i in selectedEntities)
                    i.setFaction(Unit.faction.Human);
                break;

            case 2:
                foreach (BasicMeleeUnit i in selectedEntities)
                    i.setFaction(Unit.faction.Orc);
                break;

            case 3:
                foreach (BasicMeleeUnit i in selectedEntities)
                    i.setFaction(Unit.faction.Neutral);
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
        }
    }
}