import { useEffect, useState } from "react";
import { useRef } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDisplay, faSearch, faUser } from "@fortawesome/free-solid-svg-icons";
import { faPen } from "@fortawesome/free-solid-svg-icons";
import { faTrashCan } from "@fortawesome/free-solid-svg-icons";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";
import { json, useNavigate } from "react-router-dom";
import { faCheckCircle } from "@fortawesome/free-solid-svg-icons";
import { faUserPlus } from "@fortawesome/free-solid-svg-icons";
import BarcodeScannerComponent from "react-qr-barcode-scanner";
import BarcodeGenerator from "./BarcodeGenerator";

function Members() {
  const [data, setData] = useState();
  const [error, setError] = useState();
  const [editpage, seteditpage] = useState(false);
  const [currentMember, setCurrentMember] = useState();
  const [saved, setSaved] = useState(false);
  const [refreshData, setRefreshData] = useState(false);
  const [plans, setPlans] = useState([]);
  const [currentMemberSubscriptions, setCurrentMemberSubscriptions] =
    useState();
  const [currentMemberAttendances, setCurrentMemberAttendances] = useState();

  const [selectedPlan, setSelectedPlan] = useState("");
  const [addMemberPgae, setaddMemberPgae] = useState(false);
  const [notAdded, setNotAdded] = useState(true);
  const [inputError, setInputError] = useState("");
  const [membersCount, setMembersCount] = useState(0);
  const [newMmember, setnewMember] = useState({
    name: "",
    birthday: "",
    phone: "",
    idNumber: "",
    address: "",
  });
  const [email, setEmail] = useState("");

  const [activeTab, setActiveTab] = useState("Tab1");

  const svgRef = useRef();

  const navigate = useNavigate();

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch("https://localhost:40443/members", {
          method: "GET",

          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        });
        if (response.ok) {
          var result = await response.json();

          setData(result.data);

          setMembersCount(result.recordCount);
        } else if (response.status === 401) {
          navigate("/account/login");
        }
      } catch (err) {
        setError(err.message);
        console.error(err);
      }
    }

    fetchData();
  }, [refreshData]);

  function editUserInfo(id, index) {
    setCurrentMember(data[index]);
    seteditpage((prev) => !prev);
  }

  async function handleTabClick(tab) {
    setActiveTab(tab);
  }

  async function handleSubscriptionsTabClick(tab) {
    setActiveTab(tab);

    try {
      const response = await fetch(`https://localhost:40443/plans`, {
        method: "GET",

        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });
      if (response.ok) {
        var result = await response.json();
        setPlans(result.data);
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }

    try {
      const response = await fetch(
        `https://localhost:40443/subscriptions/${currentMember.id}`,
        {
          method: "GET",

          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        }
      );
      if (response.ok) {
        var result = await response.json();

        setCurrentMemberSubscriptions(result.data);
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }
  }

  async function handleAttendancesTabClick(tab) {
    setActiveTab(tab);

    try {
      const response = await fetch(
        `https://localhost:40443/attendances/${currentMember.id}`,
        {
          method: "GET",

          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        }
      );
      if (response.ok) {
        var result = await response.json();

        setCurrentMemberAttendances(result.data);
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    setCurrentMember((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  function handleCancel() {
    seteditpage(false);
    setSaved(false);
    setaddMemberPgae(false);
    setNotAdded(true);
    setnewMember({
      name: "",
      birthday: "",
      phone: "",
      idNumber: "",
      address: "",
    });
    setActiveTab("Tab1");
    setCurrentMemberSubscriptions();
    setCurrentMemberAttendances();
  }

  const handleSave = async (e) => {
    var newobj = {
      Id: currentMember.id,
      Name: currentMember.name,
      Birthday: currentMember.birthday,
      phone: currentMember.phone,
      idNumber: currentMember.idNumber,
      Address: currentMember.addresse,
      JoinDate: currentMember.joinDate,
      AspNetUser: currentMember.AspNetUserId,
    };

    try {
      const response = await fetch("https://localhost:40443/members", {
        method: "PUT",

        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newobj),
        credentials: "include",
      });

      if (response.ok) {
        setSaved(true);
      } else if (!response.ok) {
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      console.error(err);
    }
  };

  const handleDelete = async (e) => {
    var id = e.currentTarget.parentElement.id;
    try {
      const response = await fetch(`https://localhost:40443/members/${id}`, {
        method: "DELETE",

        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });
      if (response.ok) {
      } else if (!response.ok) {
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      console.error(err);
    }
    setRefreshData((prev) => !prev);
  };

  const handleSearch = async (e) => {
    var query = e.currentTarget.value;
    try {
      const response = await fetch(
        `https://localhost:40443/members?filterquery=${query}`,
        {
          method: "GET",

          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        }
      );
      if (response.ok) {
        var result = await response.json();

        setData(result.data);
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }
  };

  const validateInputs = () => {
    if (
      !newMmember.name ||
      !newMmember.birthday ||
      !newMmember.phone ||
      !newMmember.idNumber ||
      !newMmember.address
    ) {
      setInputError("Please fill out all fields");
      return false;
    }
    setInputError("");
    return true;
  };

  const handleAddNewMember = async (e) => {
    if (!validateInputs()) return;

    try {
      const response = await fetch(`https://localhost:40443/members`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newMmember),
        credentials: "include",
      });

      if (response.ok) {
        handleCancel((prev) => !prev);
      } else if (response.status === 401) {
        navigate("/account/login");
      } else if (!response.ok) {
        setNotAdded(false);
      }
    } catch (err) {
      setNotAdded(false);
      setError(err.message);
      console.error(err);
    }
    setnewMember({
      name: "",
      birthday: "",
      phone: "",
      idNumber: "",
      address: "",
    });
    setNotAdded(true);
    setRefreshData((prev) => !prev);
  };

  const handleSelectedPlan = (e) => {
    var plan = plans.find((plan) => plan.name === e.currentTarget.value);

    setSelectedPlan(plan);
  };

  const handleAddSubscription = async () => {
    try {
      const response = await fetch(`https://localhost:40443/Subscriptions`, {
        method: "POST",

        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          memberId: currentMember.id,
          planId: selectedPlan.id,
        }),
        credentials: "include",
      });
      if (response.ok) {
        console.log("subscription added");
      } else if (!response.ok) {
        var result = await response.json();
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      console.error(err);
    }

    handleSubscriptionsTabClick("Tab2");
  };

  const downloadBarcode = () => {
    const svgElement = svgRef.current.querySelector("svg"); // Get the SVG element

    // Convert the SVG element to a string
    const svgData = new XMLSerializer().serializeToString(svgElement);

    // Create a canvas and set its dimensions
    const canvas = document.createElement("canvas");
    const context = canvas.getContext("2d");

    // Set the desired width and height
    const width = svgElement.clientWidth;
    const height = svgElement.clientHeight;

    canvas.width = width;
    canvas.height = height;

    // Convert SVG to a data URL
    const img = new Image();
    img.onload = () => {
      // Draw the SVG onto the canvas
      context.drawImage(img, 0, 0, width, height);

      // Convert the canvas content to a downloadable image
      const pngData = canvas.toDataURL("image/png");

      // Create a link and trigger download
      const downloadLink = document.createElement("a");
      downloadLink.href = pngData;
      downloadLink.download = `${currentMember.name}.png`;
      downloadLink.click();
    };
    img.src = "data:image/svg+xml;base64," + btoa(svgData);
  };

  return (
    <>
      {data ? (
        <>
          <div className="members-main">
            <h4 style={{ fontSize: "13px" }}>All Members ({membersCount})</h4>
            <div className="members-list">
              <div className="search-box">
                <div
                  className="add-member"
                  onClick={() => setaddMemberPgae((prev) => !prev)}
                >
                  <FontAwesomeIcon
                    icon={faUserPlus}
                    style={{ color: "#757994", cursor: "pointer" }}
                  />
                </div>
                <FontAwesomeIcon
                  icon={faSearch}
                  style={{
                    color: "gray",
                    fontSize: "11px",
                    zIndex: "1",
                    marginRight: "-15px",
                    position: "absolute",
                    right: "127px",
                  }}
                />
                <input placeholder="Search" onChange={handleSearch}></input>
              </div>
              {data.length === 0 ? (
                <div className="no-members">No Members Found</div>
              ) : (
                <table>
                  <thead>
                    <tr>
                      <th>Name</th>
                      <th>Mobile</th>
                      <th>ID Number</th>
                      <th>Join Date</th>
                      <th>Operation</th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.map((member, index) => (
                      <tr key={index}>
                        <td>
                          <FontAwesomeIcon
                            icon={faUser}
                            style={{
                              color: "gray",
                              marginRight: "5px",
                            }}
                          />
                          {member.name}
                        </td>
                        <td>+212 {member.phone}</td>
                        <td>{member.idNumber}</td>
                        <td>{member.joinDate}</td>
                        <td className="operations-row" id={member.id}>
                          <FontAwesomeIcon
                            className="edit-icon"
                            onClick={(e) =>
                              editUserInfo(
                                e.currentTarget.parentElement.id,
                                index
                              )
                            }
                            icon={faPen}
                            style={{ color: "blue" }}
                          />
                          <FontAwesomeIcon
                            icon={faTrashCan}
                            style={{ color: "blue" }}
                            onClick={handleDelete}
                          />
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </div>
          </div>
        </>
      ) : (
        <div className="spinner-container">
          <div className="spinner"></div>
        </div>
      )}

      {editpage ? (
        <>
          <div className="light-layer"></div>

          <div className="edit-page">
            <FontAwesomeIcon
              icon={faCircleXmark}
              className="close"
              onClick={handleCancel}
            />
            <p className="edit-header">Edit</p>

            {/* tabs */}
            <div className="tabs">
              <button
                className={`tablinks ${activeTab === "Tab1" ? "active" : ""}`}
                onClick={() => handleTabClick("Tab1")}
              >
                info
              </button>
              <button
                className={`tablinks ${activeTab === "Tab2" ? "active" : ""}`}
                onClick={() => handleSubscriptionsTabClick("Tab2")}
              >
                subscriptions
              </button>
              <button
                className={`tablinks ${activeTab === "Tab3" ? "active" : ""}`}
                onClick={() => handleAttendancesTabClick("Tab3")}
              >
                attendances
              </button>
            </div>

            {/* Tab content */}
            <div className="tabcontent">
              {activeTab === "Tab1" && (
                <>
                  <div className="tab1">
                    <div className="picture">
                      <FontAwesomeIcon
                        icon={faUserPlus}
                        style={{ fontSize: "60px", color: "gray" }}
                      />
                    </div>
                    <label for="id">Id:</label>
                    <input
                      id="id"
                      type="text"
                      name="id"
                      value={currentMember.id}
                      readOnly
                    ></input>

                    <br></br>
                    <label for="name">Name:</label>

                    <input
                      id="name"
                      type="text"
                      name="name"
                      value={currentMember.name}
                      onChange={handleInputChange}
                    ></input>

                    <br></br>

                    <label for="birthday">Birthday:</label>

                    <input
                      id="birthday"
                      type="date"
                      name="birthday"
                      value={currentMember.birthday}
                      onChange={handleInputChange}
                    ></input>
                    <br></br>
                    <label for="phone">Phone:</label>

                    <input
                      id="phone"
                      type="text"
                      name="phone"
                      value={currentMember.phone}
                      onChange={handleInputChange}
                    ></input>

                    <br></br>
                    <label for="idNumber">Id Number:</label>

                    <input
                      id="idNumber"
                      type="text"
                      name="idNumber"
                      value={currentMember.idNumber}
                      onChange={handleInputChange}
                    ></input>

                    <br></br>
                    <label for="address">Address:</label>

                    <input
                      id="address"
                      type="text"
                      name="address"
                      value={currentMember.addresse}
                      onChange={handleInputChange}
                    ></input>

                    <br></br>
                    <label for="joinDate">Join Date :</label>

                    <input
                      id="joinDate"
                      type="date"
                      name="joinDate"
                      value={currentMember.joinDate}
                      readOnly
                    ></input>
                    <br></br>

                    <div className="barcode-container" ref={svgRef}>
                      <BarcodeGenerator value={currentMember.id} />
                      <button
                        style={{
                          backgroundColor: "#15c815",
                          padding: "2px",
                          color: "white",
                          borderRadius: "3px",
                          fontWeight: "500",
                          fontSize: "8px",
                          cursor: "pointer",
                        }}
                        onClick={downloadBarcode}
                      >
                        Download
                      </button>
                    </div>
                  </div>

                  <div className="edit-buttons">
                    <button className="cancel" onClick={handleCancel}>
                      Cancel
                    </button>
                    <button className="save" onClick={handleSave}>
                      Save
                    </button>
                    {saved && (
                      <FontAwesomeIcon
                        icon={faCheckCircle}
                        style={{ color: "#00ff00", fontSize: "10px" }}
                      />
                    )}
                  </div>
                </>
              )}
              {activeTab === "Tab2" && (
                <>
                  <label
                    for="plan"
                    style={{ marginLeft: "12px", marginRight: "5px" }}
                  >
                    Choose Plan:
                  </label>
                  <input
                    list="plans"
                    name="plan"
                    id="plan"
                    style={{ marginRight: "5px" }}
                    onChange={handleSelectedPlan}
                  ></input>

                  <datalist id="plans">
                    {plans.map((plan) => (
                      <option value={plan.name}></option>
                    ))}
                  </datalist>
                  <button
                    onClick={handleAddSubscription}
                    style={{
                      padding: "2px",
                      color: "white",
                      backgroundColor: "#1ccd1c",
                      borderRadius: "3px",
                    }}
                  >
                    Add
                  </button>

                  <div className="tab2">
                    {currentMemberSubscriptions ? (
                      // currentMemberSubscriptions.length === 0 ? (
                      // <div className="no-members">No Subscriptions Found</div>
                      // ) : (
                      <>
                        <table>
                          <thead>
                            <tr>
                              <th>Id</th>
                              <th>Start Date</th>
                              <th>End Date</th>
                              <th>Status</th>
                              <th>Plan Name</th>
                              <th>Plan Price</th>
                            </tr>
                          </thead>
                          <tbody>
                            {currentMemberSubscriptions.map(
                              (subscription, index) => (
                                <tr key={index}>
                                  <td>{subscription.id}</td>
                                  <td>{subscription.startDate}</td>
                                  <td>{subscription.endDate}</td>
                                  <td>{subscription.status}</td>
                                  <td>{subscription.planName}</td>
                                  <td>{subscription.planPrice} dh</td>
                                </tr>
                              )
                            )}
                          </tbody>
                        </table>
                      </>
                    ) : (
                      // )
                      <div className="spinner-container">
                        <div className="spinner"></div>
                      </div>
                    )}
                  </div>
                </>
              )}
              {activeTab === "Tab3" && (
                <>
                  {/* <button
                    //  onClick={handleAddAttendence}
                    style={{
                      padding: "2px",
                      color: "white",
                      backgroundColor: "#1ccd1c",
                      borderRadius: "3px",
                    }}
                  >
                    Add
                  </button> */}

                  <div className="tab3">
                    {currentMemberAttendances &&
                      (currentMemberAttendances.length === 0 ? (
                        <div className="no-members">No attendances Found</div>
                      ) : (
                        <table>
                          <thead>
                            <tr>
                              <th>Id</th>
                              <th>Member ID</th>
                              <th>Checkin Time</th>
                            </tr>
                          </thead>
                          <tbody>
                            {currentMemberAttendances.map(
                              (attendance, index) => (
                                <tr key={index}>
                                  <td>{attendance.id}</td>
                                  <td>{attendance.memberId}</td>
                                  <td>{attendance.chekinTime}</td>
                                </tr>
                              )
                            )}
                          </tbody>
                        </table>
                      ))}
                  </div>
                </>
              )}
            </div>
          </div>
        </>
      ) : (
        ""
      )}

      {addMemberPgae ? (
        <>
          <div className="light-layer"></div>

          <div className="add-new-member">
            <FontAwesomeIcon
              icon={faCircleXmark}
              className="close"
              onClick={handleCancel}
            />

            <p className="header">Add New</p>
            <div className="picture">
              <FontAwesomeIcon
                icon={faUserPlus}
                style={{ fontSize: "60px", color: "gray" }}
              />
            </div>
            <div style={{ padding: "12px", fontSize: "10px" }}>
              <br></br>

              <label for="name">Name:</label>
              <input
                id="name"
                type="text"
                name="name"
                placeholder="Name"
                required
                value={newMmember.name}
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setnewMember((prevVars) => ({
                    ...prevVars,
                    name: value,
                  }));
                }}
              ></input>

              <br></br>

              <label for="birthday">Birthday:</label>

              <input
                id="birthday"
                type="date"
                name="birthday"
                required
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setnewMember((prevVars) => ({
                    ...prevVars,
                    birthday: value,
                  }));
                }}
              ></input>

              <br></br>

              <label for="phone">Phone:</label>

              <input
                id="phone"
                type="text"
                name="phone"
                placeholder="Phone Number"
                required
                // value={currentMember.phone}
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setnewMember((prevVars) => ({
                    ...prevVars,
                    phone: value,
                  }));
                }}
              ></input>

              <br></br>

              <label for="idNumber">Id Number:</label>

              <input
                id="idNumber"
                type="text"
                name="idNumber"
                placeholder="ID Number"
                required
                // value={currentMember.idNumber}
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setnewMember((prevVars) => ({
                    ...prevVars,
                    idNumber: value,
                  }));
                }}
              ></input>

              <br></br>

              <label for="addresse">Address:</label>
              <input
                id="addresse"
                type="text"
                name="addresse"
                required
                placeholder="Address"
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setnewMember((prevVars) => ({
                    ...prevVars,
                    address: value,
                  }));
                }}
              ></input>

              <br></br>
            </div>
            {inputError ? (
              <div
                style={{
                  color: "red",
                  fontSize: "10px",
                  textAlign: "center",
                  marginBottom: "10px",
                  marginTop: "-10px",
                }}
              >
                {" "}
                {inputError}
              </div>
            ) : (
              ""
            )}
            <div className="add-buttons">
              <button className="cancel" onClick={handleCancel}>
                Cancel
              </button>
              <button className="save" onClick={handleAddNewMember}>
                Add
              </button>
              {!notAdded && (
                <FontAwesomeIcon
                  icon={faCircleXmark}
                  style={{ color: "red", fontSize: "10px" }}
                />
              )}
            </div>
          </div>
        </>
      ) : (
        ""
      )}
    </>
  );
}
export default Members;
