﻿using System;
using PowerForensics.Generic;
using PowerForensics.Utilities;

namespace PowerForensics.Fat
{
    public class FileSystemInformation
    {
        #region Properties

        private readonly uint FSI_LeadSig;
        private readonly uint FSI_StrucSig;
        public readonly uint FSI_Free_Count;
        public readonly uint FSI_Nxt_Free;
        private readonly uint FSI_TrailSig;

        #endregion Properties

        #region Constructors

        private FileSystemInformation(byte[] bytes)
        {
            FSI_LeadSig = BitConverter.ToUInt32(bytes, 0);
            FSI_StrucSig = BitConverter.ToUInt32(bytes, 484);

            if (FSI_LeadSig == 0x41615252 && FSI_StrucSig == 0x61417272)
            {
                FSI_Free_Count = BitConverter.ToUInt32(bytes, 488);
                FSI_Nxt_Free = BitConverter.ToUInt32(bytes, 492);
                FSI_TrailSig = BitConverter.ToUInt32(bytes, 508);
            }
        }

        #endregion Constructors

        #region StaticMethods

        public static FileSystemInformation Get(string volume)
        {
            FatVolumeBootRecord vbr = VolumeBootRecord.Get(volume) as FatVolumeBootRecord;
            return new FileSystemInformation(DD.Get(volume, (vbr.BytesPerSector * vbr.BPB_FileSytemInfo), vbr.BytesPerSector, 1));
        }

        #endregion StaticMethods
    }
}
