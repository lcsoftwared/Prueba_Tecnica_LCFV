﻿namespace Prueba_Tecnica_LCFV
{
    public class User
    {
        public string username { get; set; } = string.Empty;
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public string type { get; set; }

    }
}
