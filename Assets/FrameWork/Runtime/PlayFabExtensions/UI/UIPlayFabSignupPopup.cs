using FrameWork.UIBinding;
using PlayFab;
using TMPro;
using UnityEngine.UI;

namespace FrameWork.PlayFabExtensions
{
    public class UIPlayFabSignupPopup : UIBase
    {
        #region ���ε�
        enum Texts
        {
            ErrorText,
        }
        enum InputFields
        {
            IDInputField,
            PasswordInputField,
            PasswordRepeatInputField,
        }
        enum Buttons
        {
            SignupButton,
            CancelButton,
        }
        #endregion

        private TextMeshProUGUI _errorText;
        private TMP_InputField _idInputField;
        private TMP_InputField _passwordInputField;
        private TMP_InputField _passwordRepeatInputField;
        private Button _signupButton;
        private UIPlayFabPopupCanvas _popupCanvas;

        public override void Show(bool isForce = false)
        {
            base.Show(isForce);

            PlayFabAuthService.OnPlayFabError += OnLoginFailed;
        }

        public override void Hide(bool isForce = false)
        {
            base.Hide(isForce);

            PlayFabAuthService.OnPlayFabError -= OnLoginFailed;
        }

        protected override void Initialize()
        {
            _popupCanvas = GetComponentInParent<UIPlayFabPopupCanvas>();

            BindText(typeof(Texts));
            BindInputField(typeof(InputFields));
            BindButton(typeof(Buttons));

            _errorText = GetText((int)Texts.ErrorText);
            _idInputField = GetInputField((int)InputFields.IDInputField);
            _passwordInputField = GetInputField((int)InputFields.PasswordInputField);
            _passwordRepeatInputField = GetInputField((int)InputFields.PasswordRepeatInputField);
            _signupButton = GetButton((int)Buttons.SignupButton);

            _signupButton.onClick.AddListener(Signup);
            GetButton((int)Buttons.CancelButton).onClick.AddListener(Cancel);
        }

        private void Signup()
        {
            var id = _idInputField.text;
            var password = _passwordInputField.text;
            var passwordRepeat = _passwordRepeatInputField.text;

            if (string.IsNullOrEmpty(id))
            {
                _errorText.text = "�̸����� �Է����ּ���.";
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                _errorText.text = "��й�ȣ�� �Է����ּ���.";
                return;
            }
            if (password != passwordRepeat)
            {
                _errorText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
                return;
            }

            _signupButton.interactable = false;
            PlayFabAuthService.Instance.Email = id;
            PlayFabAuthService.Instance.Password = password;
            PlayFabAuthService.Instance.Authenticate(Authtypes.CraeteAccount);
        }

        private void OnLoginFailed(PlayFabError result)
        {
            switch (result.Error)
            {
                case PlayFabErrorCode.InvalidEmailAddress:
                    _errorText.text = "�߸��� �̸��� �Դϴ�.";
                    break;
                case PlayFabErrorCode.InvalidPassword:
                    _errorText.text = "�߸��� ��й�ȣ �Դϴ�.";
                    break;
                case PlayFabErrorCode.InvalidEmailOrPassword:
                    _errorText.text = "�߸��� �̸��� �Ǵ� ��й�ȣ �Դϴ�.";
                    break;
                case PlayFabErrorCode.AccountNotFound:
                    _errorText.text = "������ ã�� �� �����ϴ�.";
                    break;
                default:
                    _errorText.text = "ȸ�������� �� �� �����ϴ�.\n���ͳ� ���� Ȯ���ϼ���.";
                    break;
            }

            _signupButton.interactable = true;
        }

        private void Cancel()
        {
            _popupCanvas.ShowLoginPopup();

            _idInputField.text = "";
            _passwordInputField.text = "";
            _passwordRepeatInputField.text = "";
            _errorText.text = "";
        }
    }
}