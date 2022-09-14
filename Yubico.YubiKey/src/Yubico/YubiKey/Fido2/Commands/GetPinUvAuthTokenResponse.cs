// Copyright 2022 Yubico AB
//
// Licensed under the Apache License, Version 2.0 (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Globalization;
using Yubico.Core.Iso7816;

namespace Yubico.YubiKey.Fido2.Commands
{
    /// <summary>
    /// This is the partner response class to the
    /// <see cref="GetPinTokenCommand"/>,
    /// <see cref="GetPinUvAuthTokenUsingPinCommand"/>, and
    /// <see cref="GetPinUvAuthTokenUsingUvCommand"/>, command classes.
    /// </summary>
    public class GetPinUvAuthTokenResponse : IYubiKeyResponseWithData<Memory<byte>>
    {
        private readonly ClientPinResponse _response;

        /// <summary>
        /// Constructs a new instance of the
        /// <see cref="GetPinUvAuthTokenResponse"/> class based on a response APDU
        /// provided by the YubiKey.
        /// </summary>
        /// <param name="responseApdu">
        /// A response APDU containing the CBOR response for the
        /// <c>getPinToken</c>, <c>getPinUvAuthTokenUsingPinWithPermissions</c>,
        /// and <c>getPinUvAuthTokenUsingUvWithPermissions</c> sub-commands of
        /// the <c>authenticatorClientPIN</c> CTAP2 command.
        /// </param>
        public GetPinUvAuthTokenResponse(ResponseApdu responseApdu)
        {
            _response = new ClientPinResponse(responseApdu);
        }

        /// <summary>
        /// Returns the PIN/UV Auth token generated by the YubiKey.
        /// </summary>
        /// <remarks>
        /// The token returned is encrypted using the shared secret from the
        /// protocol object used in the command.
        /// </remarks>
        public Memory<byte> GetData()
        {
            ClientPinData data = _response.GetData();

            if (data.PinUvAuthToken is null)
            {
                throw new Ctap2DataException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        ExceptionMessages.Ctap2MissingRequiredField));
            }

            return data.PinUvAuthToken.Value;
        }

        /// <inheritdoc />
        public ResponseStatus Status => _response.Status;

        /// <inheritdoc />
        public short StatusWord => _response.StatusWord;

        /// <inheritdoc />
        public string StatusMessage => _response.StatusMessage;
    }
}
