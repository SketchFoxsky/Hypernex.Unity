﻿using System.Collections.Generic;
using TMPro;
using UnityEngine.TextCore;

namespace Hypernex.Tools.Text
{
    public class Twemoji : IEmojiSheet
    {
        private List<string> unicode;

        public List<string> UnicodeCharacters
        {
            get
            {
                if (unicode == null)
                    InitializeUnicodeCharacters(Init.Instance.EmojiSprites[0]);
                return unicode;
            }
        }

        public void InitializeUnicodeCharacters(TMP_SpriteAsset spriteAsset)
        {
            List<string> unicodeCharacters = new List<string>();
            foreach (TMP_SpriteGlyph tmpSpriteGlyph in spriteAsset.spriteGlyphTable)
            {
                tmpSpriteGlyph.metrics = new GlyphMetrics(tmpSpriteGlyph.metrics.width, tmpSpriteGlyph.metrics.height,
                    0, 60, tmpSpriteGlyph.metrics.horizontalAdvance);
            }
            foreach (TMP_SpriteCharacter spriteCharacter in spriteAsset.spriteCharacterTable)
                unicodeCharacters.Add(@"\U000" + spriteCharacter.name.Split('/')[1].Replace("-", @"\U000").ToUpper());
            unicode =  unicodeCharacters;
        }

        public bool IsMatch(string unicode, string spriteAssetName)
        {
            string sub = unicode.Substring(5).Replace(@"\U000", "-");
            string compare = spriteAssetName.Split('/')[1];
            return sub.ToLower() == compare.ToLower();
        }
    }
}