using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dosymep.SimpleServices {
    /// <summary>
    /// Предоставляет доступ к языку интерфейса.
    /// </summary>
    public interface ILanguageService {

        /// <summary>
        /// Используемый язык.
        /// </summary>
        Language HostLanguage { get; }
    }

    /// <summary>
    /// Используемые языки.
    /// </summary>
    public enum Language {
        /// <summary>
        /// Английский США.
        /// </summary>
        EnglishUSA,
        /// <summary>
        /// Русский.
        /// </summary>
        Russian,
        /// <summary>
        /// Немецкий.
        /// </summary>
        German,
        /// <summary>
        /// Испанский.
        /// </summary>
        Spanish,
        /// <summary>
        /// Французский.
        /// </summary>
        French,
        /// <summary>
        /// Итальянский.
        /// </summary>
        Italian,
        /// <summary>
        /// Голландский.
        /// </summary>
        Dutch,
        /// <summary>
        /// Упрощенный китайский.
        /// </summary>
        ChineseSimplified,
        /// <summary>
        /// Традиционный китайский.
        /// </summary>
        ChineseTraditional,
        /// <summary>
        /// Японский.
        /// </summary>
        Japanese,
        /// <summary>
        /// Корейский.
        /// </summary>
        Korean,
        /// <summary>
        /// Чешский.
        /// </summary>
        Czech,
        /// <summary>
        /// Польский.
        /// </summary>
        Polish,
        /// <summary>
        /// Венгерский.
        /// </summary>
        Hungarian,
        /// <summary>
        /// Португальский.
        /// </summary>
        BrazilianPortuguese,
        /// <summary>
        /// Английский Великобритания.
        /// </summary>
        English_GB,
    }
}
