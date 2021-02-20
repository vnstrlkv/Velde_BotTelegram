using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HPCorpBot.Models.Interfaces;
namespace HPCorpBot.Models
{
    public class InlineImageGallery : IMessage
    {
        public string Message { get ; set ; }
        public string UniqueName { get; set; }       
        public List<ImageGallery> ImageGalleries { get; set; }    
        public int NextStage { get ; set ; }
        public int Stage { get ; set ; }

        public InlineImageGallery()
        {
            ImageGalleries = new List<ImageGallery>();

        }
    }
    public class ImageGallery
    {   
        
        public int StageImageGallery { get; set; }
        public string URLImage { get; set; }
        public string Descriptipon { get; set; }

        public ImageGallery(int stage, string url, string description)
        {
            StageImageGallery = stage;
            URLImage = url;
            Descriptipon = description;
        }    
            
    }
}
