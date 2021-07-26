class EditGenreForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: this.props.genre.name, description: this.props.genre.description };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.onCancelClick = this.onCancelClick.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    onCancelClick(e) {
        window.location.href = this.props.url;
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();

        const data = new FormData();
        data.append('Id', this.props.genre.id);
        data.append('Name', name);
        data.append('Description', description);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.updateUrl, true);
        xhr.onload = () => this.onCancelClick();
        xhr.send(data);
    }
    render() {
        return (
            <form className="editForm" onSubmit={this.handleSubmit} >
                <label>Genre Name:</label><br />
                <input
                    type="text"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                /><br />
                <label>Genre Description:</label><br />
                <textarea
                    rows="5"
                    cols="80"
                    type="text"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                /><br />
                <input type="submit" value="Save Genre" />
                <button type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}