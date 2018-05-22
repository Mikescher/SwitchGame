using MonoSAMFramework.Portable.Localization;
using MonoSAMFramework.Portable.Persistance;
using MonoSAMFramework.Portable.Persistance.DataFile;

namespace SwitchGame.Shared.SaveData
{
	public class UserSettings : RootDataFile
	{
		protected override SemVersion ArchiveVersion => SemVersion.VERSION_1_0_0;
		
		public int Language;

		public bool SoundsEnabled;
		public bool EffectsEnabled;
		public bool MusicEnabled;

		public UserSettings()
		{
			InitEmpty();
		}

		public void InitEmpty()
		{
			Language = L10N.LANG_EN_US;

			SoundsEnabled = true;
			EffectsEnabled = true;
			MusicEnabled = true;
		}
		
		protected override void Configure()
		{
			RegisterConstructor(() => new UserSettings());

			RegisterProperty<UserSettings>(SemVersion.VERSION_1_0_0, "sounds", o => o.SoundsEnabled,  (o, v) => o.SoundsEnabled  = v);
			RegisterProperty<UserSettings>(SemVersion.VERSION_1_0_0, "effect", o => o.EffectsEnabled, (o, v) => o.EffectsEnabled = v);
			RegisterProperty<UserSettings>(SemVersion.VERSION_1_0_0, "lang",   o => o.Language,       (o, v) => o.Language       = v);
			RegisterProperty<UserSettings>(SemVersion.VERSION_1_0_0, "music",  o => o.MusicEnabled,   (o, v) => o.MusicEnabled   = v);
		}
		
		protected override string GetTypeName()
		{
			return "USER_SETTINGS_DATA";
		}
	}
}
