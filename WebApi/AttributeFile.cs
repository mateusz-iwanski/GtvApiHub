using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    /// <summary>
    /// If AttributeType for Attribute is File you can use this enum to specify file handler.
    /// </summary>
    public enum AttributeFile
    {
        /// <summary>
        /// File handler for 3D model files.
        /// </summary>
        Model3D,

        /// <summary>
        /// File handler for image files.
        /// </summary>
        Image,

        /// <summary>
        /// File handler for video files.
        /// </summary>
        Video,

        /// <summary>
        /// File handler for audio files.
        /// </summary>
        Audio,

        /// <summary>
        /// File handler for document files.
        /// </summary>
        Document,

        /// <summary>
        /// Deklaracja CE
        /// </summary>
        deklaracja_ce,

        /// <summary>
        /// Deklaracja EAC
        /// </summary>
        deklaracja_eac,

        /// <summary>
        /// Deklaracja 037
        /// </summary>
        deklaracja_037,

        /// <summary>
        /// Etykieta energetyczna
        /// </summary>
        etykieta_energetyczna,

        /// <summary>
        /// Instrukcja obsługi
        /// </summary>
        instrukcja,

        /// <summary>
        /// Karta produktowa
        /// </summary>
        karta_produktowa,

        /// <summary>
        /// Plik fotometryczny
        /// </summary>
        pliki_fotometryczne,

        /// <summary>
        /// Karta techniczna
        /// </summary>
        karta_techniczna,

        /// <summary>
        /// Bryły 3D - STP
        /// </summary>
        solid_3d_stp,

        /// <summary>
        /// Bryły 3D - OBJ
        /// </summary>
        solid_3d_obj,

        /// <summary>
        /// Bryły 3D – 3DS
        /// </summary>
        solid_3d_3ds,

        /// <summary>
        /// Przekrój
        /// </summary>
        outline,

        /// <summary>
        /// Zdjęcie aranżacyjne
        /// </summary>
        arrangementPhotos,

        /// <summary>
        /// Zdjęcie produktowe
        /// </summary>
        productPhotos,

        /// <summary>
        /// Zdjęcie detali produktu
        /// </summary>
        productDetail,

        /// <summary>
        /// Wizualizacja opakowania
        /// </summary>
        packagingVisualization,

        /// <summary>
        /// Zdjęcie zastosowania
        /// </summary>
        inUse
    }
}
