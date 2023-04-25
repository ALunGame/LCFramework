using LCToolkit.Command;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 添加轨道命令
    /// </summary>
    internal class AddTrackCommand : ICommand
    {
        InternalTrackGroup_Element trackGroup;
        BaseTrack addTrack;
        
        public AddTrackCommand(InternalTrackGroup_Element pGroup, BaseTrack pTrack)
        {
            this.trackGroup = pGroup;
            this.addTrack = pTrack;
        }

        public void Do()
        {
            trackGroup.AddTrack(addTrack);
        }

        public void Undo()
        {
            trackGroup.RemoveTrack(addTrack);
        }
    }
    
    /// <summary>
    /// 删除轨道命令
    /// </summary>
    internal class RemoveTrackCommand : ICommand
    {
        InternalTrackGroup_Element trackGroup;
        BaseTrack removeTrack;
        
        public RemoveTrackCommand(InternalTrackGroup_Element pGroup, BaseTrack pTrack)
        {
            this.trackGroup = pGroup;
            this.removeTrack = pTrack;
        }

        public void Do()
        {
            trackGroup.RemoveTrack(removeTrack);
        }

        public void Undo()
        {
            trackGroup.AddTrack(removeTrack);
        }
    }
    
    /// <summary>
    /// 添加片段命令
    /// </summary>
    internal class AddClipCommand : ICommand
    {
        InternalTrack_Element track;
        BaseClip clip;
        
        public AddClipCommand(InternalTrack_Element pTrack, BaseClip pClip)
        {
            this.track = pTrack;
            this.clip = pClip;
        }

        public void Do()
        {
            track.AddClip(clip);
        }

        public void Undo()
        {
            track.RemoveClip(clip);
        }
    }
    
    /// <summary>
    /// 删除轨道命令
    /// </summary>
    internal class RemoveClipCommand : ICommand
    {
        InternalTrack_Element track;
        BaseClip clip;
        
        public RemoveClipCommand(InternalTrack_Element pTrack, BaseClip pClip)
        {
            this.track = pTrack;
            this.clip = pClip;
        }

        public void Do()
        {
            track.RemoveClip(clip);
        }

        public void Undo()
        {
            track.AddClip(clip);
        }
    }
}