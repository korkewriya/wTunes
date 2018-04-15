using System.Collections.Generic;

/// <summary>
/// Dictionary 型の拡張メソッドを管理するクラス
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// 指定したキーに関連付けられている値を取得します。
    /// キーが存在しない場合は既定値を返します
    /// </summary>
    public static TValue GetOrDefault<TKey, TValue>(
        this Dictionary<TKey, TValue> self,
        TKey key,
        TValue defaultValue = default(TValue))
    {
        TValue value;
        return self.TryGetValue(key, out value) ? value : defaultValue;
    }
}