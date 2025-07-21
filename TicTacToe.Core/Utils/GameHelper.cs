using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Surtility.Drawing.Components;
using Surtility.Tools;
using Surtility.Utils;
using TicTacToe.Core.Animation.Components;
using TicTacToe.Core.Cell.Components;
using TicTacToe.Core.Cell.Enums;
using TicTacToe.Core.Components;
using TicTacToe.Core.Drawing.Components;
using TicTacToe.Core.Input.Components;
using TicTacToe.Core.User.Components;

namespace TicTacToe.Core.Utils;

public static class GameHelper
{
    private static readonly string[] TextureNames = [
        "Textures/Buttons",
        "Textures/Cross",
        "Textures/Circle",
        "Textures/Grid",
        "Textures/Player1",
        "Textures/Player2",
        "Textures/CrossCursor",
        "Textures/CircleCursor",
    ];

    public static void LoadGameTextures(ContentManager content)
    {
        foreach (var textureName in TextureNames)
            content.Load<Texture2D>(textureName);
    }

    public static void InitializeGameEntities(ContentManager content)
    {
        // Создаём таймер для смены кадров на случайные, для мультяшности
        EntityGenerator.NewEntity()
            .With(new RandomFrameTimer(0.8));

        CreateGrid(content);

        CreatePlayers(content);
    }

    private static void CreateGrid(ContentManager content)
    {
        var gridTexture = content.Load<Texture2D>("Textures/Grid");
        var grid = EntityGenerator.NewEntity()
            .With(new Grid())
            .With(new CellSize(new(64, 64)))

            .With(new Sprite(gridTexture))
            .With(new SpritePosition())
            .With(new SpriteColor(Color.Black))

            .With(new SpriteScale())

            .With(new FrameCount(3))
            .With(new RandomFrame())
            .End();
    }

    private static void CreatePlayers(ContentManager content)
    {
        var crossTexture = content.Load<Texture2D>("Textures/Cross");
        var circleTexture = content.Load<Texture2D>("Textures/Circle");

        var firstPlayerIcon = content.Load<Texture2D>("Textures/Player1");
        var secondPlayerIcon = content.Load<Texture2D>("Textures/Player2");

        var crossCursor = content.Load<Texture2D>("Textures/CrossCursor");
        var circleCursor = content.Load<Texture2D>("Textures/CircleCursor");

        var firstPlayerIsCircle = Randomizer.GetHalfChance();

        var firstPlayerSign = firstPlayerIsCircle ? FigureSign.Circle : FigureSign.Cross;
        var secondPlayerSign = firstPlayerIsCircle ? FigureSign.Cross : FigureSign.Circle;
        var firstPlayerCursor = firstPlayerIsCircle ? circleCursor : crossCursor;
        var secondPlayerCursor = firstPlayerIsCircle ? crossCursor : circleCursor;
        var firstPlayerFigure = firstPlayerIsCircle ? circleTexture : crossTexture;
        var secondPlayerFigure = firstPlayerIsCircle ? crossTexture : circleTexture;

        var firstPlayer = EntityGenerator.NewEntity()
            .With(new Player())
            .With(new PlayerIcon(firstPlayerIcon))
            .With(new CursorIcon(firstPlayerCursor, origin: new(16)))
            .With(new Figure(firstPlayerSign))
            .With(new FigureTexture(firstPlayerFigure, frameCount: 8))
            .With(new SpriteColor(Color.Green))
            .With(new CreateCursor())
            .With(new WaitForInput())
            .End();

        var secondPlayer = EntityGenerator.NewEntity()
            .With(new Player())
            .With(new PlayerIcon(secondPlayerIcon))
            .With(new CursorIcon(secondPlayerCursor, origin: new(16)))
            .With(new Figure(secondPlayerSign))
            .With(new FigureTexture(secondPlayerFigure, frameCount: 8))
            .With(new SpriteColor(Color.PaleTurquoise))
            .With(new CreateCursor())
            .End();

        var firstActingPlayer = Randomizer.GetRandomItem(firstPlayer, secondPlayer);

        EntityGenerator.FillEntity(firstActingPlayer)
            .With(new Active());
    }
}
