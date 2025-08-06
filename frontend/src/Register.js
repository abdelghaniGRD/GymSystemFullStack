import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faUser,
  faUnlock,
  faEnvelope,
  faPhone,
} from "@fortawesome/free-solid-svg-icons";

function Register() {
  const [UserName, setUserName] = useState("");
  const [Email, setEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [PhoneNumber, setPhoneNumber] = useState("");
  const [Error, setError] = useState(null);
  const navigate = useNavigate();

  const HandleRegister = async () => {
    try {
      const response = await fetch("https://localhost:40443/account/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ UserName, Email, PhoneNumber, Password }),
      });
      if (response.ok) {
        navigate("/account/login");
      } else {
        setError("Registration failed");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }
  };

  return (
    <div className="register">
      <h3>Registration</h3>
      <label>
        <FontAwesomeIcon icon={faUser} style={{ color: "#009dff" }} />

        <input
          placeholder="UserName"
          onChange={(e) => setUserName(e.target.value)}
        ></input>
      </label>

      <label>
        <FontAwesomeIcon icon={faEnvelope} style={{ color: "#009dff" }} />

        <input
          placeholder="Email"
          type="email"
          onChange={(e) => setEmail(e.target.value)}
        ></input>
      </label>

      <label>
        <FontAwesomeIcon icon={faPhone} style={{ color: "#009dff" }} />

        <input
          placeholder="Phone Number"
          onChange={(e) => setPhoneNumber(e.target.value)}
        ></input>
      </label>

      <label>
        <FontAwesomeIcon icon={faUnlock} style={{ color: "#009dff" }} />

        <input
          placeholder="Password"
          type="password"
          onChange={(e) => setPassword(e.target.value)}
        ></input>
      </label>

      <button onClick={HandleRegister} className="register-button">
        Register
      </button>
      {Error && <p style={{ color: "red", fontWeight: "bold" }}>{Error}</p>}
    </div>
  );
}

export default Register;
