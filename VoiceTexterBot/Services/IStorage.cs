﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceTexterBot.Models;

namespace VoiceTexterBot.Services
{
    internal interface IStorage
    {
        Session GetSession(long chatId);
    }
}
