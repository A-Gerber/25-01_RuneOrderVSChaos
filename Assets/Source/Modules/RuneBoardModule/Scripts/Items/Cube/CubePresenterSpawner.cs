using System;
using System.Collections.Generic;
using UnityEngine;

internal class CubePresenterSpawner : Spawner<CubePresenter>
{
    private readonly List<CubePresenter> _presentersForSending = new();

    [SerializeField] private float _cubeRaycastDistance = 5f;

    private List<LocalPosition> _coordinates;

    private ICellGetable _playField;
    private ITakeable _cell;
    private int _index = 0;
    private bool _isSendForShape = true;

    internal event Action<List<CubePresenter>> CreatedCubePresenters;

    internal void Initialize(ICellGetable playField)
    {
        _playField = playField ?? throw new ArgumentNullException("playField is null", nameof(playField));
    }

    internal void CreateCubesForArea(List<LocalPosition> coordinates)
    {
        _coordinates = coordinates ?? throw new ArgumentNullException("coordinates is null", nameof(coordinates));
        _isSendForShape = false;

        for (_index = 0; _index < coordinates.Count; _index++)
        {
            if (_playField.TryGetCellByPosition(out ITakeable cell, _coordinates[_index]) && !cell.IsBusy)
            {
                _cell = cell;
                Get();
            }
        }
    }

    internal void CreateCubesForShape(List<LocalPosition> coordinates)
    {
        _coordinates = coordinates ?? throw new ArgumentNullException("coordinates is null", nameof(coordinates));
        _isSendForShape = true;
        _index = 0;

        for (_index = 0; _index < coordinates.Count; _index++)
            Get();
    }

    protected override CubePresenter Create()
    {
        CubePresenter @object = Instantiate(Prefab);
        @object.Initialize(new Cube(@object.transform, @object.Rigidbody, _cubeRaycastDistance));

        return @object;
    }

    protected override void OnRelease(CubePresenter cube)
    {
        if (cube == null)
            throw new ArgumentNullException("cube is null", nameof(cube));

        cube.SetToDefault();
        base.OnRelease(cube);

        cube.Released -= Release;
    }

    protected override void OnGet(CubePresenter cube)
    {
        if (cube == null)
            throw new ArgumentNullException("cube is null", nameof(cube));

        base.OnGet(cube);

        if (_isSendForShape)
            SendCubeViews(cube);
        else
            Transfer(cube);

        cube.Released += Release;
    }

    private void SendCubeViews(CubePresenter cube)
    {
        cube.CubeModel.SetLocalPosition(_coordinates[_index]);
        _presentersForSending.Add(cube);

        if (_index != _coordinates.Count - 1)
            return;

        CreatedCubePresenters?.Invoke(_presentersForSending);
        _presentersForSending.Clear();
    }

    private void Transfer(CubePresenter cube)
    {
        cube.transform.position = UserUtilities.TranslateInVector3(_coordinates[_index]);
        _cell.Take(cube.CubeModel);
    }
}