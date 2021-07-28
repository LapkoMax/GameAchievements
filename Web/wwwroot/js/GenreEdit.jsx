﻿class EditGenreForm extends React.Component {
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
        data.append('Name', name);
        data.append('Description', description);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.updateUrl + "?genreId=" + this.props.genreId, true);
        xhr.onload = () => this.onCancelClick();
        xhr.send(data);
    }
    render() {
        return (
            <form class="form-control" className="editForm" onSubmit={this.handleSubmit} >
                <div class="col-12 row">
                    <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Genre Name:</label>
                    <input
                        type="text"
                        class="d-flex col-lg-3 col-md-3 col-sm-3 mt-1"
                        value={this.state.name}
                        onChange={this.handleNameChange}
                    />
                </div>
                <div class="col-12 row">
                    <label class="col-lg-2 col-md-3 col-sm-3 col-form-label text-center mt-1">Genre Description:</label>
                    <div class="col-lg-10 col-md-9 col-sm-9">
                        <textarea
                            class="form-control"
                            rows="5"
                            type="text"
                            value={this.state.description}
                            onChange={this.handleDescriptionChange}
                        />
                    </div>
                </div>
                <button class="btn btn-primary col-lg-2 col-md-2 col-sm-2 mt-4" type="submit" >Save Genre</button>
                <button class="btn btn-secondary col-lg-2 col-md-2 col-sm-2 mt-4" type="button" name="Cancel" onClick={this.onCancelClick} >Cancel</button>
            </form>
        );
    }
}