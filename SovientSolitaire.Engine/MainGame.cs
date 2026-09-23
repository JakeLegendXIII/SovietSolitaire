using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SovietSolitaire.Engine.Entities;
using SovietSolitaire.Engine.Input;
using SovietSolitaire.Engine.Library;
using System;

namespace SovietSolitaire.Engine;

public class MainGame : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	private RenderTarget2D _renderTarget;
	private Rectangle _renderDestination;
	private float _scale = 1f;

	private int _nativeWidth = 1280;
	private int _nativeHeight = 800;
	private bool _isResizing;
	bool _isFullscreen = false;
	bool _isBorderless = false;
	int _width = 0;
	int _height = 0;

	EntityManager _entityManager;
	private ResetButton _resetButton;
	private ResetDialog _resetDialog;
	private RulesButton _rulesButton;
	private RulesDialog _rulesDialog;
	private WinDialog _winDialog;

	public MainGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		_graphics.GraphicsProfile = GraphicsProfile.HiDef;
		Content.RootDirectory = "Content";
		IsMouseVisible = true;

		_graphics.PreferredBackBufferWidth = _nativeWidth;
		_graphics.PreferredBackBufferHeight = _nativeHeight;
		_graphics.ApplyChanges();

		Window.Title = "Soviet Solitaire";
		Window.AllowUserResizing = true;
		Window.ClientSizeChanged += OnClientSizeChanged;
	}

	protected override void Initialize()
	{
		base.Initialize();

		_renderTarget = new RenderTarget2D(GraphicsDevice, _nativeWidth, _nativeHeight);

		CalculateRenderDestination();

		ResetHand();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		AssetManager.Load(Content);
		_resetButton = new ResetButton(new Point(20, 20));
		_resetDialog = new ResetDialog(new Point(_nativeWidth, _nativeHeight));
		_rulesButton = new RulesButton(new Point(225, 20));
		_rulesDialog = new RulesDialog(new Point(_nativeWidth, _nativeHeight));
		_winDialog = new WinDialog(new Point(_nativeWidth, _nativeHeight));
	}

	protected override void Update(GameTime gameTime)
	{
		InputManager.Update(_renderDestination, _scale, IsActive);
		UpdateGameState(gameTime);
		base.Update(gameTime);
	}

	private void UpdateGameState(GameTime gameTime)
	{
		bool backPressed = IsActive && (InputManager.IsKeyPressed(Keys.Escape)
			|| InputManager.IsButtonPressed(Buttons.Back));

		if (_resetDialog.IsOpen)
		{
			if (backPressed)
				_resetDialog.Close();
			else if (_resetDialog.Update())
				ResetHand();
			return;
		}

		if (_rulesDialog.IsOpen)
		{
			if (backPressed)
				_rulesDialog.Close();
			else
				_rulesDialog.Update(gameTime);
			return;
		}

		if (_winDialog.IsOpen)
		{
			if (_winDialog.Update())
				ResetHand();
			return;
		}

		if (backPressed)
		{
			Exit();
			return;
		}

		if (_resetButton.Update())
			_resetDialog.Show();
		else if (_rulesButton.Update())
			_rulesDialog.Show();
		else
			_entityManager.Update(gameTime);

		if (_entityManager.HasWon)
		{
			Window.Title = "Soviet Solitaire — You won!";
			_winDialog.Show();
		}
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		// Add RenterTarget2D to allow screen resizing
		GraphicsDevice.SetRenderTarget(_renderTarget);

		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

		_entityManager.Draw(_spriteBatch);
		_resetButton.Draw(_spriteBatch);
		_rulesButton.Draw(_spriteBatch);
		_rulesDialog.Draw(_spriteBatch);
		_resetDialog.Draw(_spriteBatch);
		_winDialog.Draw(_spriteBatch);

		_spriteBatch.End();

		GraphicsDevice.SetRenderTarget(null);

		_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
		_spriteBatch.Draw(_renderTarget, _renderDestination, Color.White);
		_spriteBatch.End();

		base.Draw(gameTime);
	}

	private void ResetHand()
	{
		_entityManager = new EntityManager();
		Window.Title = "Soviet Solitaire";
	}

	private void OnClientSizeChanged(object sender, EventArgs e)
	{
		if (_renderTarget is not null && !_isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
		{
			_isResizing = true;

			CalculateRenderDestination();

			_isResizing = false;
		}
	}

	private void CalculateRenderDestination()
	{
		Point size = GraphicsDevice.Viewport.Bounds.Size;

		float scaleX = (float)size.X / _renderTarget.Width;
		float scaleY = (float)size.Y / _renderTarget.Height;

		_scale = Math.Min(scaleX, scaleY);

		_renderDestination.Width = (int)(_renderTarget.Width * _scale);
		_renderDestination.Height = (int)(_renderTarget.Height * _scale);

		_renderDestination.X = (size.X - _renderDestination.Width) / 2;
		_renderDestination.Y = (size.Y - _renderDestination.Height) / 2;
	}

	// Learn MonoGame how-to Fullscreen and Borderless code
	public void ToggleFullscreen()
	{
		bool oldIsFullscreen = _isFullscreen;

		if (_isBorderless)
		{
			_isBorderless = false;
		}
		else
		{
			_isFullscreen = !_isFullscreen;
		}

		ApplyFullscreenChange(oldIsFullscreen);
	}
	public void ToggleBorderless()
	{
		bool oldIsFullscreen = _isFullscreen;

		_isBorderless = !_isBorderless;
		_isFullscreen = _isBorderless;

		ApplyFullscreenChange(oldIsFullscreen);
	}

	private void ApplyFullscreenChange(bool oldIsFullscreen)
	{
		if (_isFullscreen)
		{
			if (oldIsFullscreen)
			{
				ApplyHardwareMode();
			}
			else
			{
				SetFullscreen();
			}
		}
		else
		{
			UnsetFullscreen();
		}
	}
	private void ApplyHardwareMode()
	{
		_graphics.HardwareModeSwitch = !_isBorderless;
		_graphics.ApplyChanges();
	}
	private void SetFullscreen()
	{
		_width = Window.ClientBounds.Width;
		_height = Window.ClientBounds.Height;

		_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		// _graphics.HardwareModeSwitch = !_isBorderless;
		_graphics.HardwareModeSwitch = false;

		_graphics.IsFullScreen = true;
		_graphics.ApplyChanges();
	}
	private void UnsetFullscreen()
	{
		// Reset to default resolution, but we do have the previous Width and Height just tweaky if somebody resized
		_graphics.PreferredBackBufferWidth = _nativeWidth;
		_graphics.PreferredBackBufferHeight = _nativeHeight;

		_graphics.HardwareModeSwitch = false;
		_graphics.IsFullScreen = false;
		_graphics.ApplyChanges();
	}
}

