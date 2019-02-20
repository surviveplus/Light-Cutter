using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.UI.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INotifyDataErrorInfo
    {
        // オーバーライド・インターフェイス実装
        #region BindableBase メンバー

        // key: プロパティ名, value: エラーメッセージ文字列の一覧
        private Dictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();


        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);
            var context = new ValidationContext(this) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            if (Validator.TryValidateProperty(value, context, results))
            {
                this.errors.Remove(propertyName);
            }
            else
            {
                this.errors[propertyName] = (from r in results select r.ErrorMessage).ToList();
            } // end if

            this.OnErrorsChanged(propertyName);
            return result;
        } // end function

        #endregion

        #region INotifyDataErrorInfo メンバー
        
        /// <summary>
        /// エンティティに検証エラーがあるかどうかを示す値を取得します。
        /// </summary>
        /// <value>現在エンティティに検証エラーがある場合は true。それ以外の場合は false。</value>
        public bool HasErrors
        {
            get
            {
                return this.errors.Count > 0;
            }
        }

        /// <summary>
        /// プロパティまたはエンティティ全体の検証エラーが変更されたときに発生します。
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// 指定されたプロパティまたはエンティティ全体の検証エラーを取得します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>プロパティまたはエンティティの検証エラー。</returns>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            var results =
                (from item in errors
                 where item.Key == propertyName
                 select item.Value).FirstOrDefault();
            return results;
        }

        #endregion

        // クラスメンバー
        public bool IsValid
        {
            get
            {
                return !this.HasErrors;
            } // end get
        } // end property

        private void OnErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            this.OnPropertyChanged("IsValid");
        } // end sub

    } // end class
} // end namespace
