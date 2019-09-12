using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace U5_Map_tool
{
    #region Dict class for decompression
    class Dict
    {
        struct dict_entry
        {
            public char root;
            public int codeword;
        };

        private int contains;
        private int dict_size;
        private dict_entry[] dict;

        public Dict()
        {
            contains = 0x102;
            dict_size = 10000;
//            dict_size = 1000000;
            dict = new dict_entry[dict_size];
        }

        public void Add(char root, int codeword)
        {
            dict[contains].root = root;
            dict[contains].codeword = codeword;
            contains++;
        }

        public char GetRoot(int codeword)
        {
            return dict[codeword].root;
        }

        public int GetCodeword(int codeword)
        {
            return dict[codeword].codeword;
        }

        public void Init()
        {
            contains = 0x102;
        }
    }
    #endregion

    #region Stack class for decompression
    class Stack
    {
        private int contains;
        private int stack_size;
        private char[] stack;

        public Stack()
        {
            contains = 0;
            stack_size = 10000;
            stack = new char[stack_size];
        }

        public void Init()
        {
            contains = 0;
        }

        public bool IsEmpty()
        {
            return contains == 0;
        }

        public bool IsFull()
        {
            return contains == stack_size;
        }

        public void Push(char element)
        {
            if(!IsFull())
            {
                stack[contains] = element;
                contains++;
            }
        }

        public char Pop()
        {
            char element;
            if(!IsEmpty())
            {
                element = stack[contains - 1];
                contains--;
            }
            else
            {
                element = (char)0;
            }
            return element;
        }

        public char GetTop()
        {
            if(!IsEmpty())
            {
                return stack[contains - 1];
            }
            else
            {
                return (char)0;
            }
        }
    }
    #endregion

    class Map
    {
        private String Name;
        private Byte[,] Data;
        private Bitmap MapPic;

        public Map(List<Tile> Tiles, String src_dir, String filename, int level)
        {
            switch (filename)
            {
                #region Britannia
                case "brit.dat":
                    Name = "Britannia";
                    Data = new Byte[256, 256];
                    MapPic = new Bitmap(4096, 4096);

                    List<Byte> brit_data = new List<Byte>();
                    List<Byte> brit_ovl = LoadOVL(src_dir + "data.ovl");
                    brit_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    int brit_data_pos = 0;

                    for (int chunkY = 0; chunkY < 16; chunkY++)
                    {
                        for (int chunkX = 0; chunkX < 16; chunkX++)
                        {
                            if (brit_ovl[(chunkY * 16) + chunkX] == 0xFF)
                            {
                                for (int y = 0; y < 16; y++)
                                {
                                    for (int x = 0; x < 16; x++)
                                    {
                                        Data[(chunkY * 16 + y), (chunkX * 16 + x)] = 1;
                                    }
                                }
                            }
                            else
                            {
                                for (int y = 0; y < 16; y++)
                                {
                                    for (int x = 0; x < 16; x++)
                                    {
                                        Data[(chunkY * 16 + y), (chunkX * 16 + x)] = brit_data[brit_data_pos];
                                        brit_data_pos++;
                                    }
                                }
                            }
                        }
                    }

                    for (int y = 0; y < 256; y++)
                    {
                        for (int x = 0; x < 256; x++)
                        {
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region Underworld
                case "under.dat":
                    Name = "Underworld";
                    Data = new Byte[256, 256];
                    MapPic = new Bitmap(4096, 4096);

                    List<Byte> uw_data = new List<Byte>();
                    uw_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    int uw_data_pos = 0;

                    for (int chunkY = 0; chunkY < 16; chunkY++)
                    {
                        for (int chunkX = 0; chunkX < 16; chunkX++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                for (int x = 0; x < 16; x++)
                                {
                                    Data[(chunkY * 16 + y), (chunkX * 16 + x)] = uw_data[uw_data_pos];
                                    uw_data_pos++;
                                }
                            }
                        }
                    }

                    for (int y = 0; y < 256; y++)
                    {
                        for (int x = 0; x < 256; x++)
                        {
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region Castles
                case "castle.dat":
                    switch(level)
                    {
                        case 0:
                            Name = "Lord British's Castle - Basement";
                            break;
                        case 1:
                            Name = "Lord British's Castle - 1st Floor";
                            break;
                        case 2:
                            Name = "Lord British's Castle - 2nd Floor";
                            break;
                        case 3:
                            Name = "Lord British's Castle - Roof";
                            break;
                        case 4:
                            Name = "Lord British's Castle - Tower";
                            break;
                        case 5:
                            Name = "Lord Blackthorn's Castle - Dungeon";
                            break;
                        case 6:
                            Name = "Lord Blackthorn's Castle - 1st Floor";
                            break;
                        case 7:
                            Name = "Lord Blackthorn's Castle - 2nd Floor";
                            break;
                        case 8:
                            Name = "Lord Blackthorn's Castle - 3rd Floor";
                            break;
                        case 9:
                            Name = "Lord Blackthorn's Castle - Roof";
                            break;
                        case 10:
                            Name = "West Britany";
                            break;
                        case 11:
                            Name = "North Britanny";
                            break;
                        case 12:
                            Name = "East Britanny";
                            break;
                        case 13:
                            Name = "Paws";
                            break;
                        case 14:
                            Name = "Cove";
                            break;
                        case 15:
                            Name = "Buccaneer's Den";
                            break;
                        default:
                            break;
                    }
                    Data = new Byte[32, 32];
                    MapPic = new Bitmap(512, 512);

                    int castle_offset = level * 1024;
                    List<Byte> castle_data = new List<Byte>();
                    castle_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    for(int y = 0; y < 32; y++)
                    {
                        for(int x = 0; x < 32; x++)
                        {
                            Data[y, x] = castle_data[castle_offset + (y * 32) + x];
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region Townes
                case "towne.dat":
                    switch (level)
                    {
                        case 0:
                            Name = "Moonglow - 1st Floor";
                            break;
                        case 1:
                            Name = "Moonglow - 2nd Floor";
                            break;
                        case 2:
                            Name = "Britain - 1st Floor";
                            break;
                        case 3:
                            Name = "Britain - 2nd Floor";
                            break;
                        case 4:
                            Name = "Jhelom - 1st Floor";
                            break;
                        case 5:
                            Name = "Jhelom - 2nd Floor";
                            break;
                        case 6:
                            Name = "Yew - Basement";
                            break;
                        case 7:
                            Name = "Yew - 1st Floor";
                            break;
                        case 8:
                            Name = "Minoc - 1st Floor";
                            break;
                        case 9:
                            Name = "Minoc - 2nd Floor";
                            break;
                        case 10:
                            Name = "Trinsic - 1st Floor";
                            break;
                        case 11:
                            Name = "Trinsic - 2nd Floor";
                            break;
                        case 12:
                            Name = "Skara Brae - 1st Floor";
                            break;
                        case 13:
                            Name = "Skara Brae - 2nd Floor";
                            break;
                        case 14:
                            Name = "New Magincia - 1st Floor";
                            break;
                        case 15:
                            Name = "New Magincia - 2nd Floor";
                            break;
                        default:
                            break;
                    }
                    Data = new Byte[32, 32];
                    MapPic = new Bitmap(512, 512);

                    int towne_offset = level * 1024;
                    List<Byte> towne_data = new List<Byte>();
                    towne_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    for (int y = 0; y < 32; y++)
                    {
                        for (int x = 0; x < 32; x++)
                        {
                            Data[y, x] = towne_data[towne_offset + (y * 32) + x];
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region Dwellings
                case "dwelling.dat":
                    switch (level)
                    {
                        case 0:
                            Name = "Fogsbane - 1st Floor";
                            break;
                        case 1:
                            Name = "Fogsbane - 2nd Floor";
                            break;
                        case 2:
                            Name = "Fogsbane - 3rd Floor";
                            break;
                        case 3:
                            Name = "Stormcrow - 1st Floor";
                            break;
                        case 4:
                            Name = "Stormcrow - 2nd Floor";
                            break;
                        case 5:
                            Name = "Stormcrow - 3rd Floor";
                            break;
                        case 6:
                            Name = "Greyhaven - 1st Floor";
                            break;
                        case 7:
                            Name = "Greyhaven - 2nd Floor";
                            break;
                        case 8:
                            Name = "Greyhaven - 3rd Floor";
                            break;
                        case 9:
                            Name = "Waveguide - 1st Floor";
                            break;
                        case 10:
                            Name = "Waveguide - 2nd Floor";
                            break;
                        case 11:
                            Name = "Waveguide - 3rd Floor";
                            break;
                        case 12:
                            Name = "Iolo's Hut";
                            break;
                        case 13:
                            Name = "Spektran";
                            break;
                        case 14:
                            Name = "Sin Vraal's Hut";
                            break;
                        case 15:
                            Name = "Grendel's Hut";
                            break;
                        default:
                            break;
                    }
                    Data = new Byte[32, 32];
                    MapPic = new Bitmap(512, 512);

                    int dwelling_offset = level * 1024;
                    List<Byte> dwelling_data = new List<Byte>();
                    dwelling_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    for (int y = 0; y < 32; y++)
                    {
                        for (int x = 0; x < 32; x++)
                        {
                            Data[y, x] = dwelling_data[dwelling_offset + (y * 32) + x];
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region Keeps
                case "keep.dat":
                    switch (level)
                    {
                        case 0:
                            Name = "Ararat - 1st Floor";
                            break;
                        case 1:
                            Name = "Ararat - 2nd Floor";
                            break;
                        case 2:
                            Name = "Bordermarch - 1st Floor";
                            break;
                        case 3:
                            Name = "Bordermarch - 2nd Floor";
                            break;
                        case 4:
                            Name = "Farthing";
                            break;
                        case 5:
                            Name = "Windemere";
                            break;
                        case 6:
                            Name = "Stonegate";
                            break;
                        case 7:
                            Name = "The Lycaeum - 1st Floor";
                            break;
                        case 8:
                            Name = "The Lycaeum - 2nd Floor";
                            break;
                        case 9:
                            Name = "The Lycaeum - 3rd Floor";
                            break;
                        case 10:
                            Name = "Empath Abbey - 1st Floor";
                            break;
                        case 11:
                            Name = "Empath Abbey - 2nd Floor";
                            break;
                        case 12:
                            Name = "Empath Abbey - 3rd Floor";
                            break;
                        case 13:
                            Name = "The Serpent's Hold - Basement";
                            break;
                        case 14:
                            Name = "The Serpent's Hold - 1st Floor";
                            break;
                        case 15:
                            Name = "The Serpent's Hold - 2nd";
                            break;
                        default:
                            break;
                    }
                    Data = new Byte[32, 32];
                    MapPic = new Bitmap(512, 512);

                    int keep_offset = level * 1024;
                    List<Byte> keep_data = new List<Byte>();
                    keep_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    for (int y = 0; y < 32; y++)
                    {
                        for (int x = 0; x < 32; x++)
                        {
                            Data[y, x] = keep_data[keep_offset + (y * 32) + x];
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region MiscMaps
                case "miscmaps.dat":
                    switch (level)
                    {
                        case 0:
                            Name = "Blackthorn's Torture Chamber";
                            break;
                        case 1:
                            Name = "Shrine";
                            break;
                        case 2:
                            Name = "Codex of Ultimate Wisdom";
                            break;
                        case 3:
                            Name = "Lord British's Cell";
                            break;
                        default:
                            break;
                    }
                    Data = new Byte[11, 11];
                    MapPic = new Bitmap(176, 176);

                    int misc_offset = level * 176;
                    List<Byte> misc_data = new List<Byte>();
                    misc_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    for (int y = 0; y < 11; y++)
                    {
                        for (int x = 0; x < 11; x++)
                        {
                            Data[y, x] = misc_data[misc_offset + (y * 16) + x];
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                #region Britannia Combat
                case "brit.cbt":
                    switch (level)
                    {
                        case 0:
                            Name = "Camp";
                            break;
                        case 1:
                            Name = "Swamp";
                            break;
                        case 2:
                            Name = "Grassland";
                            break;
                        case 3:
                            Name = "Brushland";
                            break;
                        case 4:
                            Name = "Desert";
                            break;
                        case 5:
                            Name = "Forest";
                            break;
                        case 6:
                            Name = "Mountains";
                            break;
                        case 7:
                            Name = "Bridge";
                            break;
                        case 8:
                            Name = "Open floor";
                            break;
                        case 9:
                            Name = "Corridor";
                            break;
                        case 10:
                            Name = "Shadowlord's Realm";
                            break;
                        case 11:
                            Name = "Boat at sea";
                            break;
                        case 12:
                            Name = "Boat on north shore";
                            break;
                        case 13:
                            Name = "Boat on south shore";
                            break;
                        case 14:
                            Name = "Boat to boat";
                            break;
                        case 15:
                            Name = "Shoreline";
                            break;
                        default:
                            break;
                    }
                    Data = new Byte[11, 11];
                    MapPic = new Bitmap(176, 176);

                    int cbt_offset = level * 352;                                   // No idea why this is twice the amount of actual data.
                    List<Byte> cbt_data = new List<Byte>();
                    cbt_data.AddRange(File.ReadAllBytes(src_dir + filename));
                    for (int y = 0; y < 11; y++)
                    {
                        for (int x = 0; x < 11; x++)
                        {
                            Data[y, x] = cbt_data[cbt_offset + (y * 32) + x];       // 11 bytes out of 32 are used.  Why?  Who knows.
                            using (Graphics g = Graphics.FromImage(MapPic))
                            {
                                g.DrawImage(Tiles[Data[y, x]].GetTile(), (x * 16), y * 16);
                            }
                        }
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }

        private List<Byte> LoadOVL(String filename)
        {
            List<Byte> data = new List<Byte>();
            List<Byte> temp = new List<Byte>();
            data.AddRange(File.ReadAllBytes(filename));
            for(int i = 0; i < 256; i++)
            {
                temp.Add(data[14470 + i]);
            }
            return temp;
        }

        public void Save(String dest_dir)
        {
            String filename = dest_dir + "/Maps/" + Name + ".bmp";
            new FileInfo(filename).Directory.Create();
            MapPic.Save(filename);
        }
    }

    class Tile
    {
        private Color[] Palette;
        private Bitmap TilePic;

        public Tile(List<Byte> data)
        {
            SetupPalette();
            TilePic = new Bitmap(16, 16);
            int x = 0;
            int y = 0;
            for(int i = 0; i < data.Count; i ++)
            {
                int pixel1 = data[i] >> 4;
                int pixel2 = data[i] & 0xF;
                TilePic.SetPixel(x, y, Palette[pixel1]);
                x++;
                TilePic.SetPixel(x, y, Palette[pixel2]);
                x++;
                if(x == 16)
                {
                    x = 0;
                    y++;
                }
            }
        }

        public Bitmap GetTile()
        {
            return TilePic;
        }

        private void SetupPalette()
        {
            Palette = new Color[16];
            Palette[0] = Color.FromArgb(0, 0, 0);         // Black
            Palette[1] = Color.FromArgb(0, 0, 170);       // Blue
            Palette[2] = Color.FromArgb(0, 170, 0);       // Green
            Palette[3] = Color.FromArgb(0, 170, 170);     // Cryan
            Palette[4] = Color.FromArgb(170, 0, 0);       // Red
            Palette[5] = Color.FromArgb(170, 0, 170);     // Magenta
            Palette[6] = Color.FromArgb(170, 85, 0);      // Brown
            Palette[7] = Color.FromArgb(170, 170, 170);   // White/Light Gray
            Palette[8] = Color.FromArgb(85, 85, 85);      // Dark Gray/Bright Black
            Palette[9] = Color.FromArgb(85, 85, 255);     // Bright Blue
            Palette[10] = Color.FromArgb(85, 255, 85);    // Bright Green
            Palette[11] = Color.FromArgb(85, 255, 255);   // Bright Cyan
            Palette[12] = Color.FromArgb(255, 85, 85);    // Bright Red
            Palette[13] = Color.FromArgb(255, 85, 255);   // Bright Magenta
            Palette[14] = Color.FromArgb(255, 255, 85);   // Bright Yellow
            Palette[15] = Color.FromArgb(255, 255, 255);  // Bright White
        }

        public void Save(String dest_dir, int index)
        {
            String filename = dest_dir + "/Tiles/" + index + ".bmp";
            new FileInfo(filename).Directory.Create();
            TilePic.Save(filename);
        }
    }

    class Program
    {
        #region LZW Decompression
        static bool IsValidLZW(String filename)
        {
            List<Byte> data = new List<Byte>();

            if(new FileInfo(filename).Length < 6)
            {
                return false;
            }
            data.AddRange(File.ReadAllBytes(filename));
            if(data[3] != 0)
            {
                return false;
            }
            if((data[4] != 0) || ((data[5] & 1) != 1))
            {
                return false;
            }
            return true;
        }

        static Tuple<int, int> get_next_codeword(int bits_read, List<Byte> source, int codeword_size)
        {
            Byte b0, b1, b2;
            int codeword;

            b0 = source[bits_read / 8];
            b1 = source[bits_read / 8+1];
            if ((bits_read / 8 + 2) >= source.Count)
            {
                b2 = 0;
            }
            else
            {
                b2 = source[bits_read / 8 + 2];
            }

            codeword = ((b2 << 16) + (b1 << 8) + b0);
            codeword = codeword >> (bits_read % 8);
            switch(codeword_size)
            {
                case 0x9:
                    codeword = codeword & 0x1ff;
                    break;
                case 0xa:
                    codeword = codeword & 0x3ff;
                    break;
                case 0xb:
                    codeword = codeword & 0x7ff;
                    break;
                case 0xc:
                    codeword = codeword & 0xfff;
                    break;
                default:
                    Console.WriteLine("Error: weird codeword size!");
                    break;
            }
            bits_read += codeword_size;
            return Tuple.Create(codeword, bits_read);
        }

        static Byte[] Decompress(String filename)
        {
            List<Byte> data = new List<Byte>();
            data.AddRange(File.ReadAllBytes(filename));

            if (!IsValidLZW(filename))
            {
                Environment.Exit(0);
            }

            Byte[] dest;
            Byte b1 = data[0];
            Byte b2 = data[1];
            Byte b3 = data[2];
            Byte b4 = data[3];
            int dest_size = b1 + (b2 << 8) + (b3 << 16) + (b4 << 24);
            dest = new Byte[dest_size];
            data.RemoveRange(0, 4);

            bool end_marker_reached = false;
            int max_codeword_length = 12;
            int codeword_size = 9;
            int bits_read = 0;
            int next_free_codeword = 0x102;
            int dictionary_size = 0x200;

            long bytes_written = 0;
            int cW = 0;
            int pW = 0;
            char C;
            Dict dict = new Dict();
            Stack stack = new Stack();

            while(! end_marker_reached)
            {
                // get_next_codeword
                var temp = get_next_codeword(bits_read, data, codeword_size);
                cW = temp.Item1;
                bits_read = temp.Item2;
                switch(cW)
                {
                    case 0x100:
                        codeword_size = 9;
                        next_free_codeword = 0x102;
                        dictionary_size = 0x200;
                        dict.Init();
                        temp = get_next_codeword(bits_read, data, codeword_size);
                        cW = temp.Item1;
                        bits_read = temp.Item2;

                        //output_root((unsigned char)cW, destination, bytes_written);       // Next 2 lines replace this.
                        dest[bytes_written] = (Byte)cW;
                        bytes_written++;
                        break;
                    case 0x101:
                        end_marker_reached = true;
                        break;
                    default:
                        if( cW < next_free_codeword)
                        {
                            // get_string(cW);                                               // Next 9 lines replace this.
                            char root;
                            int current_codeword = cW;
                            stack.Init();
                            while(current_codeword > 0xff)
                            {
                                root = dict.GetRoot(current_codeword);
                                current_codeword = dict.GetCodeword(current_codeword);
                                stack.Push(root);
                            }
                            stack.Push((char)current_codeword);
                            // end get_string

                            C = stack.GetTop();
                            while(!stack.IsEmpty())
                            {
                                // output_root(Stack::pop(), destination, bytes_written);
                                dest[bytes_written] = (Byte)stack.Pop();
                                bytes_written++;
                                // end output_root
                            }
                            dict.Add(C, pW);

                            next_free_codeword++;
                            if(next_free_codeword >= dictionary_size)
                            {
                                if(codeword_size < max_codeword_length)
                                {
                                    codeword_size += 1;
                                    dictionary_size *= 2;
                                }
                            }
                        }
                        else
                        {
                            // get_string(pW);
                            char root;
                            int current_codeword = pW;
                            stack.Init();
                            while (current_codeword > 0xff)
                            {
                                root = dict.GetRoot(current_codeword);
                                current_codeword = dict.GetCodeword(current_codeword);
                                stack.Push(root);
                            }
                            stack.Push((char)current_codeword);
                            // end get_string

                            C = stack.GetTop();
                            while(!stack.IsEmpty())
                            {
                                // output_root(Stack::pop(), destination, bytes_written);
                                dest[bytes_written] = (Byte)stack.Pop();
                                bytes_written++;
                                // end output_root
                            }
                            // output_root(C, destination, bytes_written);
                            dest[bytes_written] = (Byte)C;
                            bytes_written++;
                            // end output_root
                            if(cW != next_free_codeword)
                            {
                                Console.WriteLine("cW != next_free_codeword!");
                                Environment.Exit(1);
                            }
                            dict.Add(C, pW);

                            next_free_codeword++;
                            if(next_free_codeword >= dictionary_size)
                            {
                                if(codeword_size < max_codeword_length)
                                {
                                    codeword_size += 1;
                                    dictionary_size *= 2;
                                }
                            }
                        }
                        break;
                }
                pW = cW;
            }
            return dest;
        }
        #endregion

        static void Main(string[] args)
        {
            String src_dir = "./U5/";
            String dest_dir = "./U5 Decomp/";
            String tiles_file = src_dir + "tiles.16";
            List<Tile> Tiles = new List<Tile>();
            Boolean Save = false;

            #region generate Tiles
            List<Byte> Bytes = new List<Byte>();
            Byte[] tiles_data = Decompress(tiles_file);
            for (int i = 0; i < tiles_data.Length; i++)
            {
                Bytes.Add(tiles_data[i]);
                if (Bytes.Count == 128)
                {
                    Tiles.Add(new Tile(Bytes));
                    Bytes.Clear();
                }
            }
            #endregion
            #region gen Britannia map
            Map brit = new Map(Tiles, src_dir, "brit.dat", 0);
            if (Save == true)
            {
                brit.Save(dest_dir);
            }
            #endregion
            #region gen Underworld map
            Map under = new Map(Tiles, src_dir, "under.dat", 0);
            if (Save == true)
            {
                under.Save(dest_dir);
            }
            #endregion
            #region gen Castle Britannia
            List<Map> BritishCastle = new List<Map>();
            for(int i = 0; i < 5; i++)
            {
                BritishCastle.Add(new Map(Tiles, src_dir, "castle.dat", i));
            }
            if (Save == true)
            {
                foreach(Map map in BritishCastle)
                {
                    map.Save(dest_dir);
                }
            }
            #endregion
            #region gen Blackthorn's castle
            List<Map> BlackthornCastle = new List<Map>();
            for (int i = 5; i < 10; i++)
            {
                BlackthornCastle.Add(new Map(Tiles, src_dir, "castle.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in BlackthornCastle)
                {
                    map.Save(dest_dir);
                }
            }
            #endregion
            #region gen villages
            Map WBritanny = new Map(Tiles, src_dir, "castle.dat", 10);
            if (Save == true)
            {
                WBritanny.Save(dest_dir);
            }
            Map NBritanny = new Map(Tiles, src_dir, "castle.dat", 11);
            if (Save == true)
            {
                NBritanny.Save(dest_dir);
            }
            Map EBritanny = new Map(Tiles, src_dir, "castle.dat", 12);
            if (Save == true)
            {
                EBritanny.Save(dest_dir);
            }
            Map Paws = new Map(Tiles, src_dir, "castle.dat", 13);
            if (Save == true)
            {
                Paws.Save(dest_dir);
            }
            Map Cove = new Map(Tiles, src_dir, "castle.dat", 14);
            if (Save == true)
            {
                Cove.Save(dest_dir);
            }
            Map BuccDen = new Map(Tiles, src_dir, "castle.dat", 15);
            if (Save == true)
            {
                BuccDen.Save(dest_dir);
            }
            #endregion
            #region gen Townes
            List<Map> Moonglow = new List<Map>();
            for (int i = 0; i < 2; i++)
            {
                Moonglow.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Moonglow)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Britain = new List<Map>();
            for(int i = 2; i < 4; i++)
            {
                Britain.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Britain)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Jhelom = new List<Map>();
            for (int i = 4; i < 6; i++)
            {
                Jhelom.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Jhelom)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Yew = new List<Map>();
            for (int i = 6; i < 8; i++)
            {
                Yew.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Yew)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Minoc = new List<Map>();
            for (int i = 8; i < 10; i++)
            {
                Minoc.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Minoc)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Trinsic = new List<Map>();
            for (int i = 10; i < 12; i++)
            {
                Trinsic.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Trinsic)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> SkaraBrae = new List<Map>();
            for (int i = 12; i < 14; i++)
            {
                SkaraBrae.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in SkaraBrae)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> NewMagincia = new List<Map>();
            for (int i = 14; i < 16; i++)
            {
                NewMagincia.Add(new Map(Tiles, src_dir, "towne.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in NewMagincia)
                {
                    map.Save(dest_dir);
                }
            }
            #endregion
            #region gen Dwellings
            List<Map> Fogsbane = new List<Map>();
            for(int i = 0; i < 3; i++)
            {
                Fogsbane.Add(new Map(Tiles, src_dir, "dwelling.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Fogsbane)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Stormcrow = new List<Map>();
            for (int i = 3; i < 6; i++)
            {
                Stormcrow.Add(new Map(Tiles, src_dir, "dwelling.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Stormcrow)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Greyhaven = new List<Map>();
            for (int i = 6; i < 9; i++)
            {
                Greyhaven.Add(new Map(Tiles, src_dir, "dwelling.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Greyhaven)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Waveguide = new List<Map>();
            for (int i = 9; i < 12; i++)
            {
                Waveguide.Add(new Map(Tiles, src_dir, "dwelling.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Waveguide)
                {
                    map.Save(dest_dir);
                }
            }
            Map IolosHut = new Map(Tiles, src_dir, "dwelling.dat", 12);
            if (Save == true)
            {
                IolosHut.Save(dest_dir);
            }
            Map Spektran = new Map(Tiles, src_dir, "dwelling.dat", 13);
            if (Save == true)
            {
                Spektran.Save(dest_dir);
            }
            Map SinVrallsHut = new Map(Tiles, src_dir, "dwelling.dat", 14);
            if (Save == true)
            {
                SinVrallsHut.Save(dest_dir);
            }
            Map GrendelsHut = new Map(Tiles, src_dir, "dwelling.dat", 15);
            if (Save == true)
            {
                GrendelsHut.Save(dest_dir);
            }
            #endregion
            #region gen Keeps
            List<Map> Ararat = new List<Map>();
            for(int i = 0; i < 2; i++)
            {
                Ararat.Add(new Map(Tiles, src_dir, "keep.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Ararat)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> Bordermarch = new List<Map>();
            for(int i = 2; i < 4; i++)
            {
                Bordermarch.Add(new Map(Tiles, src_dir, "keep.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Bordermarch)
                {
                    map.Save(dest_dir);
                }
            }
            Map Farthing = new Map(Tiles, src_dir, "keep.dat", 4);
            if (Save == true)
            {
                Farthing.Save(dest_dir);
            }
            Map Windemere = new Map(Tiles, src_dir, "keep.dat", 5);
            if (Save == true)
            {
                Windemere.Save(dest_dir);
            }
            Map Stonegate = new Map(Tiles, src_dir, "keep.dat", 6);
            if (Save == true)
            {
                Stonegate.Save(dest_dir);
            }
            List<Map> Lycaeum = new List<Map>();
            for(int i = 7; i < 10; i++)
            {
                Lycaeum.Add(new Map(Tiles, src_dir, "keep.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in Lycaeum)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> EmpathAbbey = new List<Map>();
            for(int i = 10; i < 13; i++)
            {
                EmpathAbbey.Add(new Map(Tiles, src_dir, "keep.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in EmpathAbbey)
                {
                    map.Save(dest_dir);
                }
            }
            List<Map> SerpentsHold = new List<Map>();
            for(int i = 13; i < 16; i++)
            {
                SerpentsHold.Add(new Map(Tiles, src_dir, "keep.dat", i));
            }
            if (Save == true)
            {
                foreach (Map map in SerpentsHold)
                {
                    map.Save(dest_dir);
                }
            }
            #endregion
            #region gen MiscMaps
            List<Map> MiscMaps = new List<Map>();
            for(int i = 0; i < 4; i++)
            {
                MiscMaps.Add(new Map(Tiles, src_dir, "miscmaps.dat", i));
            }
            if(Save == true)
            {
                foreach(Map map in MiscMaps)
                {
                    map.Save(dest_dir);
                }
            }
            #endregion
            #region gen Combat maps
            List<Map> CombatMaps = new List<Map>();
            for (int i = 0; i < 16; i++)
            {
                CombatMaps.Add(new Map(Tiles, src_dir, "brit.cbt", i));
            }
            if (Save == true)
            {
                foreach (Map map in CombatMaps)
                {
                    map.Save(dest_dir);
                }
            }
            #endregion
            #region gen U4 shapes.ega
            List<Byte> u4_data = new List<Byte>();
            u4_data.AddRange(File.ReadAllBytes("./U4/shapes.ega"));
            int u4off = 0;
            int u5off = 0;
            for (int u4tile = 0; u4tile < 256; u4tile++)
            {
                if (u4tile == 0) { u5off = 1 * 128; }
                if (u4tile == 1) { u5off = 2 * 128; }
                if (u4tile == 2) { u5off = 3 *128; }
                if (u4tile == 3) { u5off = 4 *128; }
                if (u4tile == 4) { u5off = 5 *128; }
                if (u4tile == 5) { u5off = 8 *128; }
                if (u4tile == 6) { u5off = 10 *128; }
                if (u4tile == 7) { u5off = 11 *128; }
                if (u4tile == 8) { u5off = 12 *128; }
                if (u4tile == 9) { u5off = 24 *128; }
                if (u4tile == 10) { u5off = 20 *128; }
                if (u4tile == 11) { u5off = 21 *128; }
                if (u4tile == 12) { u5off = 19 *128; }
                if (u4tile == 13) { u5off = 61 *128; }
                if (u4tile == 14) { u5off = 62 *128; }
                if (u4tile == 15) { u5off = 63 *128; }
                if (u4tile == 16) { u5off = 291 *128; }
                if (u4tile == 17) { u5off = 288 *128; }
                if (u4tile == 18) { u5off = 289 *128; }
                if (u4tile == 19) { u5off = 290 *128; }
                if (u4tile == 20) { u5off = 273 *128; }
                if (u4tile == 21) { u5off = 272 *128; }
                if (u4tile == 22) { u5off = 69 *128; }
                if (u4tile == 23) { u5off = 29 *128; }
                if (u4tile == 24) { continue; }
                if (u4tile == 25) { u5off = 64 *128; }
                if (u4tile == 26) { u5off = 64 *128; }
                if (u4tile == 27) { u5off = 200 *128; }
                if (u4tile == 28) { u5off = 201 *128; }
                if (u4tile == 29) { continue; }
                if (u4tile == 30) { u5off = 25 *128; }
                if (u4tile == 31) { u5off = 284 *128; }
                if (u4tile == 32) { u5off = 320 *128; }
                if (u4tile == 33) { u5off = 322 *128; }
                if (u4tile == 34) { u5off = 348 *128; }
                if (u4tile == 35) { u5off = 350 *128; }
                if (u4tile == 36) { u5off = 368 *128; }
                if (u4tile == 37) { u5off = 370 *128; }
                if (u4tile == 38) { u5off = 336 *128; }
                if (u4tile == 39) { u5off = 338 *128; }
                if (u4tile == 40) { u5off = 324 *128; }
                if (u4tile == 41) { u5off = 326 *128; }
                if (u4tile == 42) { u5off = 328 *128; }
                if (u4tile == 43) { u5off = 330 *128; }
                if (u4tile == 44) { u5off = 360 *128; }
                if (u4tile == 45) { u5off = 362 *128; }
                if (u4tile == 46) { u5off = 364 *128; }
                if (u4tile == 47) { u5off = 366 *128; }
                if (u4tile == 48) { u5off = 70 *128; }
                if (u4tile == 49) { u5off = 231 *128; }
                if (u4tile == 50) { u5off = 230 *128; }
                if (u4tile == 51) { u5off = 228 *128; }
                if (u4tile == 52) { u5off = 229 *128; }
                if (u4tile == 53) { u5off = 66 *128; }
                if (u4tile == 54) { u5off = 67 *128; }
                if (u4tile == 55) { u5off = 76 *128; }
                if (u4tile == 56) { u5off = 286 *128; }
                if (u4tile == 57) { u5off = 77 *128; }
                if (u4tile == 58) { u5off = 185 *128; }
                if (u4tile == 59) { u5off = 184 *128; }
                if (u4tile == 60) { u5off = 257 *128; }
                if (u4tile == 61) { u5off = 178 *128; }
                if (u4tile == 62) { u5off = 68 *128; }
                if (u4tile == 63) { u5off = 64 *128; }
                if (u4tile == 64) { u5off = 220 *128; }
                if (u4tile == 65) { u5off = 220 *128; }
                if (u4tile == 66) { u5off = 220 *128; }
                if (u4tile == 67) { u5off = 220 *128; }
                if (u4tile == 68) { u5off = 488 *128; }
                if (u4tile == 69) { u5off = 491 *128; }
                if (u4tile == 70) { u5off = 490 *128; }
                if (u4tile == 71) { u5off = 489 *128; }
                if (u4tile == 72) { u5off = 254 *128; }
                if (u4tile == 73) { u5off = 78 *128; }
                if (u4tile == 74) { u5off = 65 *128; }
                if (u4tile == 75) { u5off = 179 *128; }
                if (u4tile == 76) { u5off = 143 *128; }
                if (u4tile == 77) { u5off = 0 *128; }
                if (u4tile == 78) { u5off = 41 *128; }
                if (u4tile == 79) { u5off = 0 *128; }
                if (u4tile == 80) { u5off = 369 *128; }
                if (u4tile == 81) { u5off = 371 *128; }
                if (u4tile == 82) { u5off = 337 *128; }
                if (u4tile == 83) { u5off = 339 *128; }
                if (u4tile == 84) { u5off = 349 *128; }
                if (u4tile == 85) { u5off = 351 *128; }
                if (u4tile == 86) { u5off = 345 *128; }
                if (u4tile == 87) { u5off = 347 *128; }
                if (u4tile == 88) { u5off = 365 *128; }
                if (u4tile == 89) { u5off = 367 *128; }
                if (u4tile == 90) { u5off = 361 *128; }
                if (u4tile == 91) { u5off = 363 *128; }
                if (u4tile == 92) { continue; }
                if (u4tile == 93) { continue; }
                if (u4tile == 94) { u5off = 380 *128; }
                if (u4tile == 95) { u5off = 382 *128; }
                if (u4tile == 96) { continue; }
                if (u4tile == 97) { continue; }
                if (u4tile == 98) { continue; }
                if (u4tile == 99) { continue; }
                if (u4tile == 100) { continue; }
                if (u4tile == 101) { continue; }
                if (u4tile == 102) { continue; }
                if (u4tile == 103) { continue; }
                if (u4tile == 104) { continue; }
                if (u4tile == 105) { continue; }
                if (u4tile == 106) { continue; }
                if (u4tile == 107) { continue; }
                if (u4tile == 108) { continue; }
                if (u4tile == 109) { continue; }
                if (u4tile == 110) { continue; }
                if (u4tile == 111) { continue; }
                if (u4tile == 112) { continue; }
                if (u4tile == 113) { continue; }
                if (u4tile == 114) { continue; }
                if (u4tile == 115) { continue; }
                if (u4tile == 116) { continue; }
                if (u4tile == 117) { continue; }
                if (u4tile == 118) { continue; }
                if (u4tile == 119) { continue; }
                if (u4tile == 120) { continue; }
                if (u4tile == 121) { continue; }
                if (u4tile == 122) { continue; }
                if (u4tile == 123) { continue; }
                if (u4tile == 124) { continue; }
                if (u4tile == 125) { continue; }
                if (u4tile == 126) { continue; }
                if (u4tile == 127) { u5off = 79 *128; }
                if (u4tile == 128) { u5off = 303 *128; }
                if (u4tile == 129) { u5off = 300 *128; }
                if (u4tile == 130) { u5off = 301 *128; }
                if (u4tile == 131) { u5off = 302 *128; }
                if (u4tile == 132) { continue; }
                if (u4tile == 133) { continue; }
                if (u4tile == 134) { u5off = 388 *128; }
                if (u4tile == 135) { u5off = 390 *128; }
                if (u4tile == 136) { u5off = 392 *128; }
                if (u4tile == 137) { u5off = 394 *128; }
                if (u4tile == 138) { u5off = 384 *128; }
                if (u4tile == 139) { u5off = 386 *128; }
                if (u4tile == 140) { u5off = 492 *128; }
                if (u4tile == 141) { u5off = 494 *128; }
                if (u4tile == 142) { continue; }
                if (u4tile == 143) { continue; }
                if (u4tile == 144) { u5off = 400 *128; }
                if (u4tile == 145) { u5off = 401 *128; }
                if (u4tile == 146) { u5off = 402 *128; }
                if (u4tile == 147) { u5off = 403 *128; }
                if (u4tile == 148) { u5off = 404 *128; }
                if (u4tile == 149) { u5off = 405 *128; }
                if (u4tile == 150) { u5off = 406 *128; }
                if (u4tile == 151) { u5off = 407 *128; }
                if (u4tile == 152) { u5off = 408 *128; }
                if (u4tile == 153) { u5off = 409 *128; }
                if (u4tile == 154) { u5off = 410 *128; }
                if (u4tile == 155) { u5off = 411 *128; }
                if (u4tile == 156) { u5off = 412 *128; }
                if (u4tile == 157) { u5off = 413 *128; }
                if (u4tile == 158) { u5off = 414 *128; }
                if (u4tile == 159) { u5off = 415 *128; }
                if (u4tile == 160) { u5off = 416 *128; }
                if (u4tile == 161) { u5off = 417 *128; }
                if (u4tile == 162) { u5off = 418 *128; }
                if (u4tile == 163) { u5off = 419 *128; }
                if (u4tile == 164) { u5off = 448 *128; }
                if (u4tile == 165) { u5off = 449 *128; }
                if (u4tile == 166) { u5off = 450 *128; }
                if (u4tile == 167) { u5off = 451 *128; }
                if (u4tile == 168) { u5off = 420 *128; }
                if (u4tile == 169) { u5off = 421 *128; }
                if (u4tile == 170) { u5off = 422 *128; }
                if (u4tile == 171) { u5off = 423 *128; }
                if (u4tile == 172) { u5off = 424 *128; }
                if (u4tile == 173) { u5off = 425 *128; }
                if (u4tile == 174) { u5off = 426 *128; }
                if (u4tile == 175) { u5off = 427 *128; }
                if (u4tile == 176) { u5off = 428 *128; }
                if (u4tile == 177) { u5off = 429 *128; }
                if (u4tile == 178) { u5off = 430 *128; }
                if (u4tile == 179) { u5off = 431 *128; }
                if (u4tile == 180) { u5off = 444 *128; }
                if (u4tile == 181) { u5off = 445 *128; }
                if (u4tile == 182) { u5off = 446 *128; }
                if (u4tile == 183) { u5off = 447 *128; }
                if (u4tile == 184) { u5off = 432 *128; }
                if (u4tile == 185) { u5off = 433 *128; }
                if (u4tile == 186) { u5off = 434 *128; }
                if (u4tile == 187) { u5off = 435 *128; }
                if (u4tile == 188) { continue; }
                if (u4tile == 189) { continue; }
                if (u4tile == 190) { continue; }
                if (u4tile == 191) { continue; }
                if (u4tile == 192) { u5off = 484 *128; }
                if (u4tile == 193) { u5off = 485 *128; }
                if (u4tile == 194) { u5off = 486 *128; }
                if (u4tile == 195) { u5off = 487 *128; }
                if (u4tile == 196) { u5off = 452 *128; }
                if (u4tile == 197) { u5off = 453 *128; }
                if (u4tile == 198) { u5off = 454 *128; }
                if (u4tile == 199) { u5off = 455 *128; }
                if (u4tile == 200) { u5off = 324 * 128; }
                if (u4tile == 201) { u5off = 325 *128; }
                if (u4tile == 202) { u5off = 326 *128; }
                if (u4tile == 203) { u5off = 327 *128; }
                if (u4tile == 204) { u5off = 456 *128; }
                if (u4tile == 205) { u5off = 457 *128; }
                if (u4tile == 206) { u5off = 458 *128; }
                if (u4tile == 207) { u5off = 459 *128; }
                if (u4tile == 208) { u5off = 460 *128; }
                if (u4tile == 209) { u5off = 461 *128; }
                if (u4tile == 210) { u5off = 462 *128; }
                if (u4tile == 211) { u5off = 463 *128; }
                if (u4tile == 212) { u5off = 464 *128; }
                if (u4tile == 213) { u5off = 465 *128; }
                if (u4tile == 214) { u5off = 466 *128; }
                if (u4tile == 215) { u5off = 467 *128; }
                if (u4tile == 216) { continue; }
                if (u4tile == 217) { continue; }
                if (u4tile == 218) { continue; }
                if (u4tile == 219) { continue; }
                if (u4tile == 220) { u5off = 468 *128; }
                if (u4tile == 221) { u5off = 469 *128; }
                if (u4tile == 222) { u5off = 470 *128; }
                if (u4tile == 223) { u5off = 471 *128; }
                if (u4tile == 224) { u5off = 320 *128; }
                if (u4tile == 225) { u5off = 321 *128; }
                if (u4tile == 226) { u5off = 322 *128; }
                if (u4tile == 227) { u5off = 323 *128; }
                if (u4tile == 228) { u5off = 508 *128; }
                if (u4tile == 229) { u5off = 509 *128; }
                if (u4tile == 230) { u5off = 510 *128; }
                if (u4tile == 231) { u5off = 511 *128; }
                if (u4tile == 232) { continue; }
                if (u4tile == 233) { continue; }
                if (u4tile == 234) { continue; }
                if (u4tile == 235) { continue; }
                if (u4tile == 236) { u5off = 480 *128; }
                if (u4tile == 237) { u5off = 481 *128; }
                if (u4tile == 238) { u5off = 482 *128; }
                if (u4tile == 239) { u5off = 483 *128; }
                if (u4tile == 240) { u5off = 496 *128; }
                if (u4tile == 241) { u5off = 497 *128; }
                if (u4tile == 242) { u5off = 498 *128; }
                if (u4tile == 243) { u5off = 499 *128; }
                if (u4tile == 244) { continue; }
                if (u4tile == 245) { continue; }
                if (u4tile == 246) { continue; }
                if (u4tile == 247) { continue; }
                if (u4tile == 248) { u5off = 476 *128; }
                if (u4tile == 249) { u5off = 477 *128; }
                if (u4tile == 250) { u5off = 478 *128; }
                if (u4tile == 251) { u5off = 479 *128; }
                if (u4tile == 252) { u5off = 472 *128; }
                if (u4tile == 253) { u5off = 473 *128; }
                if (u4tile == 254) { u5off = 474 *128; }
                if (u4tile == 255) { u5off = 475 *128; }

                u4off = u4tile * 128;
                for ( int i = 0; i < 128; i++)
                {
                    u4_data[u4off + i] = tiles_data[u5off + i];
                }
            }
            File.WriteAllBytes("./shapes.ega.u5", u4_data.ToArray());
            #endregion
        }
    }
}
