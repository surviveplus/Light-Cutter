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
        #region BindableBase members

        /// <summary>
        /// <para xml:lang="en">
        /// key : property name, value : list of error message text
        /// </para>
        /// <para xml:lang="ja">
        /// key: プロパティ名, value: エラーメッセージ文字列の一覧
        /// </para>
        /// </summary>
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

        #region INotifyDataErrorInfo members

        /// <summary>
        /// <para xml:lang="en">
        /// Gets a value that indicates whether the entity has validation errors.
        /// </para>
        /// <para xml:lang="ja">
        /// エンティティに検証エラーがあるかどうかを示す値を取得します。
        /// </para>
        /// </summary>
        /// <value>
        /// <para xml:lang="en">
        /// true if the entity currently has validation errors; otherwise, false.
        /// </para>
        /// <para xml:lang="ja">
        /// 現在エンティティに検証エラーがある場合は true。それ以外の場合は false。
        /// </para>
        /// </value>
        public bool HasErrors
        {
            get
            {
                return this.errors.Count > 0;
            }
        }

        /// <summary>
        /// <para xml:lang="en">
        /// Occurs when the validation errors have changed for a property or for the entire entity.
        /// </para>
        /// <para xml:lang="ja">
        /// プロパティまたはエンティティ全体の検証エラーが変更されたときに発生します。
        /// </para>
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// <para xml:lang="en">
        /// Gets the validation errors for a specified property or for the entire entity.
        /// </para>
        /// <para xml:lang="ja">
        /// 指定されたプロパティまたはエンティティ全体の検証エラーを取得します。
        /// </para>
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or null or Empty, to retrieve entity-level errors.
        /// </param>
        /// <returns>
        /// <para xml:lang="en">
        /// The validation errors for the property or entity.
        /// </para>
        /// <para xml:lang="ja">
        /// プロパティまたはエンティティの検証エラー。
        /// </para>
        /// </returns>
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            var results =
                (from item in errors
                 where item.Key == propertyName
                 select item.Value).FirstOrDefault();
            return results;
        }

        #endregion


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
