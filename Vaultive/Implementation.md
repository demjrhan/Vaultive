
EF does not accept primitive type list like List<string> due to mapping issues. To fix that I created AudioLanguage and SubtitleLanguage in order to fix this. But this will cause to each AudioOption/SubtitleOption have their own unique language like Spanish Turkish etc. It will look like duplication but if I make a generic class like Language for these option and pass it as many to many connection it will not support composition.

