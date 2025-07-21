using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surtility.Cleaning;
using Surtility.Drawing.Components;
using Surtility.Drawing.Systems;
using Surtility.Timing;
using Surtility.Tools;
using Surtility.Tweening.Systems;
using Surtility.Utils;
using TicTacToe.Core.Animation.Components;
using TicTacToe.Core.Animation.Systems;
using TicTacToe.Core.Cell.Components;
using TicTacToe.Core.Cell.Enums;
using TicTacToe.Core.Components;
using TicTacToe.Core.Drawing.Components;
using TicTacToe.Core.Drawing.Systems;
using TicTacToe.Core.Events;
using TicTacToe.Core.Input.Components;
using TicTacToe.Core.Input.Systems;
using TicTacToe.Core.User.Components;
using TicTacToe.Core.Utils;

namespace TicTacToe.Main;

public class TicTacToeGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private EcsWorld _ecsWorld;
    private EcsSystems _updateSystems;
    private EcsSystems _drawSystems;

    private readonly InputDevices _inputDevices = new();

    private Texture2D _inputIcons;

    public TicTacToeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _ecsWorld = new EcsWorld();
        EntityGenerator.Initialize(_ecsWorld);

        _drawSystems = new EcsSystems(_ecsWorld);
        _updateSystems = new EcsSystems(_ecsWorld);

        var fieldSize = 64 * 3;

        _graphics.PreferredBackBufferWidth = fieldSize;
        _graphics.PreferredBackBufferHeight = fieldSize;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _inputIcons = Content.Load<Texture2D>("Textures/Buttons");

        var gridTexture = Content.Load<Texture2D>("Textures/Grid");

        var firstPlayerIcon = Content.Load<Texture2D>("Textures/Player1");
        var secondPlayerIcon = Content.Load<Texture2D>("Textures/Player2");

        EntityGenerator.NewEntity()
            .With(new Grid())
            .With(new CellSize(new(64, 64)))

            .With(new Sprite(gridTexture))
            .With(new SpritePosition())
            .With(new SpriteColor(Color.Black))

            .With(new FrameCount(3))
            .With(new RandomFrame())
            .End();
            
        EntityGenerator.NewEntity()
            .With(new RandomFrameTimer(0.8));

        var firstPlayerIsCircle = Randomizer.GetHalfChance();

        var firstPlayerSign = firstPlayerIsCircle ? FigureSign.Circle : FigureSign.Cross;
        var secondPlayerSign = firstPlayerIsCircle ? FigureSign.Cross : FigureSign.Circle;

        var firstPlayer = EntityGenerator.NewEntity()
            .With(new Player())
            .With(new PlayerIcon(firstPlayerIcon))
            .With(new Figure(firstPlayerSign))
            .With(new WaitForInput())
            .With(new SpriteColor(Color.Green))
            .End();

        var secondPlayer = EntityGenerator.NewEntity()
            .With(new Player())
            .With(new PlayerIcon(secondPlayerIcon))
            .With(new Figure(secondPlayerSign))
            .With(new SpriteColor(Color.PaleTurquoise))
            .End();

        InitializeEcsSystems();
    }

    protected override void Update(GameTime gameTime)
    {
        DeltaTime.UpdateTime(gameTime);

        _inputDevices.Update();

        _updateSystems.Run();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _drawSystems.Run();
    }

    private void InitializeEcsSystems()
    {
        _updateSystems
            .Add(new CreateInputAssignScreenSystem(_inputIcons))

            .Add(new CreateCursorSystem())
            .Add(new UpdateCursorPositionSystem(_inputDevices, Window.ClientBounds.Size))

            .Add(new AssignInputSystem(_inputDevices, Window.ClientBounds.Size))
            .Add(new CleanAssignScreenSystem())

            .Add(new AnimateSequentialFrameSystem())
            .Add(new RandomizeExistingFrameSystem())
            .Add(new AssignRandomFrameSystem())

            .Add(new UpdateTweenTimeSystem())
            .Add(new UpdateTweenValueSystem<Vector2>(Vector2.Lerp))
            .Add(new UpdateTweenValueSystem<Color>(Color.Lerp))

            .Add(new AnimateComponentSystem<SpriteScale, Vector2>(MapFunctions.SaveSpriteScale))
            .Add(new AnimateComponentSystem<SpritePosition, Vector2>(MapFunctions.SaveSpritePosition))
            .Add(new AnimateComponentSystem<SpriteColor, Color>(MapFunctions.SaveSpriteColor))

            .Add(new LoopTweenSystem<Vector2>())
            .Add(new DeleteTweenSystem())
            .Add(new RemoveEntityWithComponentSystem<EventMarker>())
            .Init();

        _drawSystems
            .Add(new CreateSpriteFrameSystem())
            .Add(new CreateSpriteBorderSystem())

            .Add(new BeginDrawSystem(_spriteBatch, SamplerState.PointClamp))
            .Add(new DrawSpriteSystem(_spriteBatch))
            .Add(new EndDrawSystem(_spriteBatch))
            .Init();
    }
}
