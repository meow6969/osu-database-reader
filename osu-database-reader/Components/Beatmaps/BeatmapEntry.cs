using System;
using System.Collections.Generic;
using osu.Shared;
using osu.Shared.Serialization;
using osu_database_reader.BinaryFiles;

namespace osu_database_reader.Components.Beatmaps
{
    /// <summary>
    /// An entry in <see cref="OsuDb"/> with information about a beatmap.
    /// </summary>
    public class BeatmapEntry : ISerializable
    {
        public string Artist, ArtistUnicode;
        public string Title, TitleUnicode;
        public string Creator;  //mapper
        public string Version;  //difficulty name
        public string AudioFileName;
        public string BeatmapChecksum;
        public string BeatmapFileName;
        public SubmissionStatus RankedStatus;
        public ushort CountHitCircles, CountSliders, CountSpinners;
        public DateTime LastModifiedTime;
        public float ApproachRate, CircleSize, HPDrainRate, OveralDifficulty;
        public double SliderVelocity;
        public Dictionary<Mods, double> DiffStarRatingStandard, DiffStarRatingTaiko, DiffStarRatingCtB, DiffStarRatingMania;
        public int DrainTimeSeconds;    //NOTE: in s
        public int TotalTime;           //NOTE: in ms
        public int AudioPreviewTime;    //NOTE: in ms
        public List<TimingPoint> TimingPoints;
        public int BeatmapId, BeatmapSetId, ThreadId;
        public Ranking GradeStandard, GradeTaiko, GradeCtB, GradeMania;
        public short OffsetLocal;
        public float StackLeniency;
        public GameMode GameMode;
        public string SongSource, SongTags;
        public short OffsetOnline;
        public string TitleFont;
        public bool Unplayed;
        public DateTime LastPlayed;
        public bool IsOsz2;
        public string FolderName;
        public DateTime LastCheckAgainstOsuRepo;
        public bool IgnoreBeatmapSounds, IgnoreBeatmapSkin, DisableStoryBoard, DisableVideo, VisualOverride;
        public short OldUnknown1;   //unused
        public int LastEditTime;
        public byte ManiaScrollSpeed;

        private int _version;

        public static BeatmapEntry ReadFromReader(SerializationReader r, int version) {
            var e = new BeatmapEntry {
                _version = version,
            };

            e.ReadFromStream(r);

            return e;
        }

        public void ReadFromStream(SerializationReader r)
        {
            Artist = r.ReadString();
            if (_version >= OsuVersions.FirstOsz2)
                ArtistUnicode = r.ReadString();
            Title = r.ReadString();
            if (_version >= OsuVersions.FirstOsz2)
                TitleUnicode = r.ReadString();
            Creator = r.ReadString();
            Version = r.ReadString();
            AudioFileName = r.ReadString();
            BeatmapChecksum = r.ReadString();   //always 32 in length, so the 2 preceding bytes in the file are practically wasting space
            BeatmapFileName = r.ReadString();

            RankedStatus = (SubmissionStatus)r.ReadByte();
            CountHitCircles = r.ReadUInt16();
            CountSliders = r.ReadUInt16();
            CountSpinners = r.ReadUInt16();
            LastModifiedTime = r.ReadDateTime();

            if (_version >= OsuVersions.FloatDifficultyValues)
            {
                ApproachRate = r.ReadSingle();
                CircleSize = r.ReadSingle();
                HPDrainRate = r.ReadSingle();
                OveralDifficulty = r.ReadSingle();
            }
            else
            {
                ApproachRate = r.ReadByte();
                CircleSize = r.ReadByte();
                HPDrainRate = r.ReadByte();
                OveralDifficulty = r.ReadByte();
            }

            SliderVelocity = r.ReadDouble();

            if (_version >= OsuVersions.FloatDifficultyValues)
            {
                DiffStarRatingStandard = r.ReadDictionary<Mods, double>();
                DiffStarRatingTaiko = r.ReadDictionary<Mods, double>();
                DiffStarRatingCtB = r.ReadDictionary<Mods, double>();
                DiffStarRatingMania = r.ReadDictionary<Mods, double>();

                // TODO: there may be different reading behavior for versions before 20190204, 20200916, 20200504 and 20191024 here.
            }

            DrainTimeSeconds = r.ReadInt32();
            TotalTime = r.ReadInt32();
            AudioPreviewTime = r.ReadInt32();

            TimingPoints = r.ReadSerializableList<TimingPoint>();
            BeatmapId = r.ReadInt32();
            BeatmapSetId = r.ReadInt32();
            ThreadId = r.ReadInt32();

            GradeStandard = (Ranking)r.ReadByte();
            GradeTaiko = (Ranking)r.ReadByte();
            GradeCtB = (Ranking)r.ReadByte();
            GradeMania = (Ranking)r.ReadByte();

            OffsetLocal = r.ReadInt16();
            StackLeniency = r.ReadSingle();
            GameMode = (GameMode)r.ReadByte();

            SongSource = r.ReadString();
            SongTags = r.ReadString();
            OffsetOnline = r.ReadInt16();
            TitleFont = r.ReadString();
            Unplayed = r.ReadBoolean();
            LastPlayed = r.ReadDateTime();

            IsOsz2 = r.ReadBoolean();
            FolderName = r.ReadString();
            LastCheckAgainstOsuRepo = r.ReadDateTime();

            IgnoreBeatmapSounds = r.ReadBoolean();
            IgnoreBeatmapSkin = r.ReadBoolean();
            DisableStoryBoard = r.ReadBoolean();
            DisableVideo = r.ReadBoolean();
            VisualOverride = r.ReadBoolean();
            if (_version < OsuVersions.FloatDifficultyValues)
                OldUnknown1 = r.ReadInt16();
            LastEditTime = r.ReadInt32();
            ManiaScrollSpeed = r.ReadByte();
        }

        public void WriteToStream(SerializationWriter w)
        {
            throw new NotImplementedException();
        }
    }
}
