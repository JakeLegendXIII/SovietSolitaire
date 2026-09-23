using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SovietSolitaire.Engine.Library;
using System.Collections.Generic;

namespace SovietSolitaire.Engine.Sound;

public class SoundManager : IGameEntity
{
	private int _soundtrackIndex = -1;
	private List<SoundEffectInstance> _soundtracks = new List<SoundEffectInstance>();
	// private Dictionary<Type, SoundBankItem> _soundBank = new Dictionary<Type, SoundBankItem>();		

	public void Update(GameTime gameTime)
	{

	}

	public void Draw(SpriteBatch spriteBatch)
	{

	}

	public void SetSoundtrack(List<SoundEffectInstance> tracks)
	{
		_soundtracks = tracks;
		_soundtrackIndex = _soundtracks.Count - 1;
	}

	public void PlaySoundtrack()
	{
		var nbTracks = _soundtracks.Count;

		if (nbTracks <= 0)
		{
			return;
		}

		var currentTrack = _soundtracks[_soundtrackIndex];
		var nextTrack = _soundtracks[(_soundtrackIndex + 1) % nbTracks];

		if (currentTrack.State == SoundState.Stopped)
		{
			nextTrack.Play();
			_soundtrackIndex++;

			if (_soundtrackIndex >= _soundtracks.Count)
			{
				_soundtrackIndex = 0;
			}
		}
	}

	public void StopSoundtrack()
	{
		var nbTracks = _soundtracks.Count;

		if (nbTracks <= 0)
		{
			return;
		}

		var currentTrack = _soundtracks[_soundtrackIndex];
		var nextTrack = _soundtracks[(_soundtrackIndex + 1) % nbTracks];

		if (currentTrack.State == SoundState.Playing)
		{
			currentTrack.Stop();
		}
	}
}