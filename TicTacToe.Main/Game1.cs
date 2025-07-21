using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surtility.Cleaning;
using Surtility.Drawing.Components;
using Surtility.Drawing.Systems;
using Surtility.Timing;
using Surtility.Tools;
using Surtility.Tweening.Systems;
using TicTacToe.Core.Animation.Systems;
using TicTacToe.Core.Drawing.Systems;
using TicTacToe.Core.Events;
using TicTacToe.Core.Input.Systems;
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

        GameHelper.LoadGameTextures(Content);

        _inputIcons = Content.Load<Texture2D>("Textures/Buttons");

        GameHelper.InitializeGameEntities(Content);

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
